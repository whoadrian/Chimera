using Chimera.AI.Utilities;
using UnityEngine;

namespace Chimera.AI
{
    public class CommandAttackNode : Node
    {
        public CommandAttackNode(BehaviourTree tree) : base(tree)
        {
        }

        public override State Evaluate()
        {
            if (_tree.GetContext(Context.AttackCommandKey) != null)
            {
                var enemyTransform = (Transform)_tree.GetContext(Context.AttackCommandKey);
                if (enemyTransform != null)
                {
                    _tree.SetContext(Context.EnemyTargetKey, enemyTransform);
                    _tree.SetContext(Context.AttackCommandKey, null);
                    _tree.SetContext(Context.MoveToCommandKey, null);
                    
                    _state = State.Success;
                    return _state;
                }
            }

            _state = State.Failure;
            return _state;
        }
    }
}