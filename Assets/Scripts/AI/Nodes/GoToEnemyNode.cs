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
            var enemyTarget = (Transform)_tree.GetContext(Context.EnemyTargetKey);
            if (enemyTarget != null)
            {
                _tree.actor.navMeshAgent.SetDestination(enemyTarget.position);
            }
            else
            {
                _state = State.Failure;
                return _state;
            }

            _tree.actor.animator.SetBool(_tree.actor.config.walkAnimBool, true);
            
            _state = State.Running;
            return _state;
        }
    }
}