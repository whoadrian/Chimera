using System.Collections.Generic;

namespace Chimera.AI
{
    /// <summary>
    /// Behaviour Tree node. Inherit from this for your own custom Node logic.
    /// </summary>
    public abstract class Node
    {
        public enum State
        {
            Running, Failure, Success
        }

        // Parent tree reference
        protected BehaviourTree _tree;
        
        // Current state of the node
        protected State _state;
        
        // Parent node, if any
        protected Node _parent = null;
        
        // Children nodes
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