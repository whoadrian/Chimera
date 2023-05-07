namespace Chimera.AI
{
    public class SelectorNode : Node
    {
        public SelectorNode(BehaviourTree tree) : base(tree)
        {
        }
        
        public override State Evaluate()
        {
            foreach (var childNode in _children)
            {
                switch (childNode.Evaluate())
                {
                    case State.Failure:
                        continue;
                    case State.Success:
                        _state = State.Success;
                        return _state;
                    case State.Running:
                        _state = State.Running;
                        return _state;
                    default:
                        continue;
                }
            }

            _state = State.Failure;
            return _state;
        }
    }
}