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

        private Dictionary<string, object> _context = new();

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

        protected Node GetRootParent()
        {
            var p = _parent;
            while (p._parent != null)
            {
                p = p._parent;
            }

            return p;
        }

        public void SetContext(string key, object value)
        {
            _context[key] = value;
        }

        public object GetContext(string key)
        {
            object data = null;
            if (_context.TryGetValue(key, out data))
            {
                return data;
            }

            var node = _parent;
            while (node != null)
            {
                data = node.GetContext(key);
                if (data != null)
                {
                    return data;
                }

                node = node._parent;
            }

            return null;
        }

        public bool ClearContext(string key)
        {
            if (_context.ContainsKey(key))
            {
                _context.Remove(key);
                return true;
            }

            var node = _parent;
            while (node != null)
            {
                var cleared = node.ClearContext(key);
                if (cleared)
                {
                    return true;
                }

                node = node._parent;
            }

            return false;
        }
    }
}