using System;
using UnityEngine;

namespace Chimera.AI
{
    [RequireComponent(typeof(Character))]
    public class BehaviourTree : MonoBehaviour
    {
        public BehaviourTreeBlueprint blueprint;

        [HideInInspector]
        public Character character;
        private Node _root = null;

        private void Start()
        {
            character = GetComponent<Character>();
            _root = BuildTree(blueprint.root);
        }

        private void Update()
        {
            _root?.Evaluate();
        }

        private Node BuildTree(NodeBlueprint parent)
        {
            var parentNode = BuildNode(parent.type);

            foreach (var c in parent.children)
            {
                var childNode = BuildTree(c);
                parentNode.AddChild(childNode);
            }

            return parentNode;
        }

        private Node BuildNode(string nodeTypeName)
        {
            if (nodeTypeName == string.Empty)
            {
                Debug.LogError($"Node Type is empty! {nodeTypeName}");
                return null;
            }

            var nodeType = Type.GetType(nodeTypeName);
            if (nodeType == null)
            {
                Debug.LogError($"Node Type not recognised! {nodeTypeName}");
                return null;
            }
            
            var node = (Node)Activator.CreateInstance(nodeType);
            node.SetTree(this);
            
            return node;
        }
    }
}