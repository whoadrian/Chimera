
namespace Chimera.AI
{
    public class SequenceNode : Node
    {
        public override State Evaluate()
        {
            bool anyChildIsRunning = false;
            
            foreach (Node n in _children)
            {
                switch (n.Evaluate())
                {
                    case State.Failure:
                        _state = State.Failure;
                        return _state;
                    case State.Success:
                        continue;
                    case State.Running:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        _state = State.Success;
                        return _state;
                }
            }

            _state = anyChildIsRunning ? State.Running : State.Success;
            return _state;
        }
    }
}