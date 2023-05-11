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
            var commandData = _tree.GetCommandContext(Context.Commands.AttackCommandKey);
            if (commandData != null)
            {
                var enemy = (Transform)commandData;
                if (enemy != null)
                {
                    _tree.SetNodesContext(Context.Nodes.EnemyTargetKey, enemy);
                    _tree.SetCommandContext(Context.Commands.AttackCommandKey, null);
                
                    _state = State.Success;
                    return _state;
                }
            }
            
            _state = State.Failure;
            return _state;
        }
    }
}