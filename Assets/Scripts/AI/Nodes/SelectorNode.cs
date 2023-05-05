namespace Chimera.AI
{
    public class SelectorNode : Node
    {
        public override State Evaluate()
        {
            foreach (Node n in _children)
            {
                switch (n.Evaluate())
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