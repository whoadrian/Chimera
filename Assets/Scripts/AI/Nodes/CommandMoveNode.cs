using Chimera.AI.Utilities;
using UnityEngine;

namespace Chimera.AI
{
    public class CommandMoveNode : Node
    {
        private bool _moving = false;
        
        public CommandMoveNode(BehaviourTree tree) : base(tree)
        {
        }

        public override State Evaluate()
        {
            if (_tree.GetContext(Context.MoveToCommandKey) != null)
            {
                _tree.SetContext(Context.EnemyTargetKey, null);
                
                var pos = (Vector3)_tree.GetContext(Context.MoveToCommandKey);
                
                _tree.actor.navMeshAgent.SetDestination(pos);
                _tree.actor.animator.SetBool(_tree.actor.config.walkAnimBool, true);
                _tree.SetContext(Context.DestinationKey, pos);

                if (_tree.actor.navMeshAgent.remainingDistance < 0.5f)
                {
                    _tree.SetContext(Context.MoveToCommandKey, null);
                }
                
                _moving = true;
                _state = State.Running;
                return _state;
            }

            _state = State.Failure;
            return _state;
        }
    }
}