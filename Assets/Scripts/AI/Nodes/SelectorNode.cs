namespace Chimera.AI
{
    /// <summary>
    /// Similar to an OR gate, stops at first Successful or Running child
    /// </summary>
    public class SelectorNode : Node
    {
        public SelectorNode(BehaviourTree tree) : base(tree)
        {
        }
        
        public override State Evaluate()
        {
            // Similar to an OR gate, stops at first Successful or Running child
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