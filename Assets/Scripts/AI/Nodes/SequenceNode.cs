﻿
namespace Chimera.AI
{
    /// <summary>
    /// Sequence of events. Similar to an AND gate, stops at first Running or Failed child. Tries to have all children return Success.
    /// </summary>
    public class SequenceNode : Node
    {
        public SequenceNode(BehaviourTree tree) : base(tree)
        {
        }
        
        public override State Evaluate()
        {
            // Similar to an AND gate, stops at first Running or Failed child. Tries to have all children return Success.
            foreach (var childNode in _children)
            {
                switch (childNode.Evaluate())
                {
                    case State.Failure:
                        _state = State.Failure;
                        return _state;
                    case State.Success:
                        continue;
                    case State.Running:
                        _state = State.Running;
                        return _state;
                    default:
                        _state = State.Success;
                        return _state;
                }
            }

            _state = State.Success;
            return _state;
        }
    }
}