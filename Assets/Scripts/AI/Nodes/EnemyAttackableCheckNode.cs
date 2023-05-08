using Chimera.AI.Utilities;
using UnityEngine;

namespace Chimera.AI
{
    public class EnemyAttackableCheckNode : Node
    {
        public EnemyAttackableCheckNode(BehaviourTree tree) : base(tree)
        {
        }
        
        public override State Evaluate()
        {
            var enemyTarget = (Transform)_tree.GetContext(Context.EnemyTargetKey);
            
            if (enemyTarget != null)
            {
                if (Vector3.Distance(_tree.actor.transform.position,
                        enemyTarget.position) < _tree.actor.config.attackRange)
                {
                    if (_tree.actor.navMeshAgent != null && _tree.actor.navMeshAgent.remainingDistance > 0f)
                    {
                        _tree.actor.navMeshAgent.SetDestination(_tree.actor.transform.position);
                    }
                    
                    _tree.actor.animator?.SetBool(_tree.actor.config.attackAnimBool, true);
                    
                    _state = State.Success;
                    return _state;
                }
            }

            _state = State.Failure;
            return _state;
        }
    }
}