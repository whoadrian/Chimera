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
            var enemyTarget = (Transform)GetContext(Context.EnemyTargetKey);
            _tree.character.animator.SetBool(_tree.character.config.attackAnimBool, false);
            
            if (enemyTarget != null)
            {
                if (Vector3.Distance(_tree.character.transform.position,
                        enemyTarget.position) < _tree.character.config.attackRange)
                {
                    if (_tree.character.navMeshAgent.remainingDistance > 0f)
                    {
                        _tree.character.navMeshAgent.SetDestination(_tree.character.transform.position);
                    }
                    
                    _tree.character.animator.SetBool(_tree.character.config.attackAnimBool, true);
                    
                    _state = State.Success;
                    return _state;
                }
            }

            _state = State.Failure;
            return _state;
        }
    }
}