using Chimera.AI.Utilities;
using UnityEngine;

namespace Chimera.AI
{
    public class CommandMoveNode : Node
    {
        public CommandMoveNode(BehaviourTree tree) : base(tree)
        {
        }

        public override State Evaluate()
        {
            var commandData = _tree.GetCommandContext(Context.Commands.MoveToCommandKey);

            if (commandData != null)
            {
                var moveToPos = (Vector3)commandData;
                
                _tree.actor.animator.SetBool(_tree.actor.config.walkAnimBool, true);
                _tree.SetNodesContext(Context.Nodes.DestinationKey, moveToPos);
                _tree.SetNodesContext(Context.Nodes.EnemyTargetKey, null);
                
                if (_tree.actor.navMeshAgent.destination != moveToPos)
                {
                    _tree.actor.navMeshAgent.SetDestination(moveToPos);
                }
                
                if (Vector3.Distance(_tree.actor.transform.position, moveToPos) < 0.5f)
                {
                    _tree.SetCommandContext(Context.Commands.MoveToCommandKey, null);
                    
                    _state = State.Success;
                    return _state;
                }
                
                _state = State.Running;
                return _state;
            }
            
            _state = State.Failure;
            return _state;
        }
    }
}