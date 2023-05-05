using Chimera.AI.Utilities;
using UnityEngine;

namespace Chimera.AI
{
    public class EnemyAttackableCheckNode : Node
    {
        public override State Evaluate()
        {
            var enemyTarget = (Transform)GetContext(Context.EnemyTargetKey);
            if (enemyTarget != null)
            {
                if (Vector3.Distance(_tree.character.transform.position,
                        enemyTarget.position) < _tree.character.config.attackRange)
                {
                    _state = State.Success;
                    return _state;
                }
            }

            _state = State.Failure;
            return _state;
        }
    }
}