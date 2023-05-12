using Chimera.AI.Utilities;
using UnityEngine;
using UnityEngine.AI;

namespace Chimera.AI
{
    /// <summary>
    /// Checks if enemy is in attackable range.
    /// </summary>
    public class EnemyAttackableCheckNode : Node
    {
        public EnemyAttackableCheckNode(BehaviourTree tree) : base(tree)
        {
        }
        
        public override State Evaluate()
        {
            // Get enemy from context
            var enemyTarget = (Transform)_tree.GetNodesContext(Context.NodeKey.EnemyTarget);
            
            // Check if enemy is within attack range and visible on the navmesh
            if (enemyTarget != null &&
                !NavMesh.Raycast(_tree.actor.transform.position, enemyTarget.position, out var hitData, Layers.DefaultLayerMask) &&
                Vector3.SqrMagnitude(_tree.actor.transform.position - enemyTarget.position) < _tree.actor.config.attackRange * _tree.actor.config.attackRange)
            {
                // Enemy is attackable
                _state = State.Success;
                return _state;
            }

            // Enemy doesn't exist or is not attackable yet
            _state = State.Failure;
            return _state;
        }
    }
}