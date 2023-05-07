using System.Collections.Generic;

namespace Chimera.AI
{
    public abstract class Node
    {
        public enum State
        {
            Running, Failure, Success
        }

        protected BehaviourTree _tree;
        protected State _state;
        protected Node _parent = null;
        protected List<Node> _children = new();

        public Node(BehaviourTree tree)
        {
            _tree = tree;
        }
        
        public void AddChild(Node child)
        {
            child._parent = this;
            child._tree = _tree;
            _children.Add(child);
        }

        public abstract State Evaluate();
    }
}