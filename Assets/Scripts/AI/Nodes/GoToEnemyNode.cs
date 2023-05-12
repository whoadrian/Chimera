using Chimera.AI.Utilities;
using UnityEngine;

namespace Chimera.AI
{
    /// <summary>
    /// Moves the actor to the enemy position
    /// </summary>
    public class GoToEnemyNode : Node
    {
        public GoToEnemyNode(BehaviourTree tree) : base(tree)
        {
        }
        
        public override State Evaluate()
        {
            // Check for existing enemy in the context
            var enemyTarget = (Transform)_tree.GetNodesContext(Context.NodeKey.EnemyTarget);
            if (enemyTarget == null)
            {
                _tree.actor.navMeshAgent.SetDestination(_tree.actor.transform.position);
            
                // No enemy
                _state = State.Failure;
                return _state;
            }

            // Set navmesh destination
            if (_tree.actor.navMeshAgent.destination != enemyTarget.position)
            {
                _tree.actor.navMeshAgent.SetDestination(enemyTarget.position);
            }

            // Set walk animation
            _tree.actor.animator.SetBool(_tree.actor.config.walkAnimBool, true);
            
            // Set destination context
            _tree.SetNodesContext(Context.NodeKey.Destination, enemyTarget.position);

            _state = State.Running;
            return _state;
        }
    }
}