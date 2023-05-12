using Chimera.AI.Utilities;
using UnityEngine;

namespace Chimera.AI
{
    /// <summary>
    /// Used when issuing a player-command to this actor. Force-sets an enemy.
    /// </summary>
    public class CommandAttackNode : Node
    {
        public CommandAttackNode(BehaviourTree tree) : base(tree)
        {
        }

        public override State Evaluate()
        {
            // Check command context for enemy
            var commandData = _tree.GetCommandContext(Context.CommandKey.AttackCommand);
            if (commandData != null)
            {
                // Get enemy transform
                var enemy = (Transform)commandData;
                if (enemy != null)
                {
                    // Set context enemy
                    _tree.SetNodesContext(Context.NodeKey.EnemyTarget, enemy);
                    
                    // Remove attack command data from the command context, otherwise this node will keep triggering
                    _tree.SetCommandContext(Context.CommandKey.AttackCommand, null);
                
                    _state = State.Success;
                    return _state;
                }
            }
            
            // No enemy
            _state = State.Failure;
            return _state;
        }
    }
}