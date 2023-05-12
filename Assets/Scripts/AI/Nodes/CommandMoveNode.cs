using Chimera.AI.Utilities;
using UnityEngine;

namespace Chimera.AI
{
    /// <summary>
    /// Player-issued command, move to a point.
    /// </summary>
    public class CommandMoveNode : Node
    {
        public CommandMoveNode(BehaviourTree tree) : base(tree)
        {
        }

        public override State Evaluate()
        {
            // Check command context for move command
            var commandData = _tree.GetCommandContext(Context.CommandKey.MoveToCommand);
            if (commandData != null)
            {
                // Get move to position
                var moveToPos = (Vector3)commandData;
                
                // Set walk animation
                _tree.actor.animator.SetBool(_tree.actor.config.walkAnimBool, true);
                
                // Set destination context
                _tree.SetNodesContext(Context.NodeKey.Destination, moveToPos);
                
                // Remove any enemy data. This player-command makes the actor forget of any enemy.
                _tree.SetNodesContext(Context.NodeKey.EnemyTarget, null);
                
                // Set agent destination on the navmesh
                if (_tree.actor.navMeshAgent.destination != moveToPos)
                {
                    _tree.actor.navMeshAgent.SetDestination(moveToPos);
                }
                
                // Check if destination has been reached
                if (Vector3.Distance(_tree.actor.transform.position, moveToPos) < 1.5f)
                {
                    // Destination reached, remove move command data from context
                    _tree.SetCommandContext(Context.CommandKey.MoveToCommand, null);
                    
                    _state = State.Success;
                    return _state;
                }
                
                // We are moving
                _state = State.Running;
                return _state;
            }
            
            // No move command
            _state = State.Failure;
            return _state;
        }
    }
}