using Chimera.AI.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace Chimera.AI
{
    /// <summary>
    /// Checks if current enemy is within the fov range. If there is no enemy, will try to find one within the fov range.
    /// </summary>
    public class EnemyVisibleCheckNode : Node
    {
        private Transform _actorTransform;

        public EnemyVisibleCheckNode(BehaviourTree tree) : base(tree)
        {
            _actorTransform = _tree.actor.transform;
        }

        public override State Evaluate()
        {
            // Get existing enemy from context
            var enemyTarget = (Transform)_tree.GetNodesContext(Context.Nodes.EnemyTargetKey);
            if (enemyTarget == null)
            {
                // No enemy, check for enemies within fov range
                var colliders =
                    Physics.OverlapSphere(_actorTransform.position, _tree.actor.config.fovRange, Layers.ActorLayerMask);
                
                foreach (var c in colliders)
                {
                    // Check navmesh visibility
                    if (NavMesh.Raycast(_tree.actor.transform.position, c.transform.position, out _,
                            Layers.DefaultLayerMask))
                    {
                        continue;
                    }
                    
                    // Check if object is an actor
                    var otherActorBehaviour = c.GetComponent<Actor>();
                    if (otherActorBehaviour == null)
                    {
                        continue;
                    }

                    // Check if actor belongs to other faction
                    if (otherActorBehaviour.faction != _tree.actor.faction)
                    {
                        // Enemy found, store in context
                        _tree.SetNodesContext(Context.Nodes.EnemyTargetKey, otherActorBehaviour.transform);

                        _state = State.Success;
                        return _state;
                    }
                }

                // No enemy found
                _state = State.Failure;
                return _state;
            }
            
            // We already have an enemy in the context
            _state = State.Success;
            return _state;
        }
    }
}