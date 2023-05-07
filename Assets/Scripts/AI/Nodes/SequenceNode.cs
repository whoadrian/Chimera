
namespace Chimera.AI
{
    public class SequenceNode : Node
    {
        public SequenceNode(BehaviourTree tree) : base(tree)
        {
        }
        
        public override State Evaluate()
        {
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