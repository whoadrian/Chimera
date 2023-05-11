using Chimera.AI.Utilities;
using UnityEngine;

namespace Chimera.AI
{
    public class GoToEnemyNode : Node
    {
        public GoToEnemyNode(BehaviourTree tree) : base(tree)
        {
        }
        
        public override State Evaluate()
        {
            var enemyTarget = (Transform)_tree.GetNodesContext(Context.Nodes.EnemyTargetKey);
            if (enemyTarget == null)
            {
                _state = State.Failure;
                return _state;
            }

            if (_tree.actor.navMeshAgent.destination != enemyTarget.position)
            {
                _tree.actor.navMeshAgent.SetDestination(enemyTarget.position);
            }

            _tree.actor.animator.SetBool(_tree.actor.config.walkAnimBool, true);
            _tree.SetNodesContext(Context.Nodes.DestinationKey, enemyTarget.position);

            _state = State.Running;
            return _state;
        }
    }
}