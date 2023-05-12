using Chimera.AI.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace Chimera.AI
{
    /// <summary>
    /// Tries to find the closest enemy within the field-of-view range.
    /// Does nothing if we already have an enemy target.
    /// </summary>
    public class EnemyVisibleCheckNode : Node
    {
        // re-usable collider buffer for non-alloc physics check.
        // since we're running on a single thread, static is fine
        private static Collider[] _colliderBuffer = new Collider[256];

        public EnemyVisibleCheckNode(BehaviourTree tree) : base(tree)
        {
        }

        public override State Evaluate()
        {
            // Get existing enemy from context
            var enemyTarget = (Transform)_tree.GetNodesContext(Context.NodeKey.EnemyTarget);
            if (enemyTarget != null)
            {
                // We already have an enemy in the context
                _state = State.Success;
                return _state;
            }

            // No enemy, check for enemies within fov range
            var size = Physics.OverlapSphereNonAlloc(_tree.actor.transform.position, _tree.actor.config.fovRange,
                _colliderBuffer, Layers.ActorLayerMask);

            var minSqrDistance = float.MaxValue;

            for (var i = 0; i < size; ++i)
            {
                var c = _colliderBuffer[i];

                // Check if object is an actor
                var otherActorBehaviour = c.GetComponent<Actor>();
                if (otherActorBehaviour == null)
                {
                    continue;
                }
                
                // Check if actor belongs to other faction, and is closest
                var sqrDistance = Vector3.SqrMagnitude(_tree.actor.transform.position - c.transform.position);
                if (otherActorBehaviour.faction != _tree.actor.faction && sqrDistance < minSqrDistance)
                {
                    // Check navmesh visibility
                    if (NavMesh.Raycast(_tree.actor.transform.position, c.transform.position, out _,
                            Layers.DefaultLayerMask))
                    {
                        continue;
                    }
                    
                    // Enemy found, store in context
                    _tree.SetNodesContext(Context.NodeKey.EnemyTarget, otherActorBehaviour.transform);
                    minSqrDistance = sqrDistance;
                }
            }

            // Enemy was found
            if (minSqrDistance < float.MaxValue)
            {
                _state = State.Success;
                return _state;
            }

            // No enemy found
            _state = State.Failure;
            return _state;
        }
    }
}