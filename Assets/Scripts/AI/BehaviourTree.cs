using System;
using UnityEngine;

namespace Chimera.AI
{
    [RequireComponent(typeof(Actor))]
    public class BehaviourTree : MonoBehaviour
    {
        public BehaviourTreeBlueprint blueprint;

        [HideInInspector]
        public Actor actor;
        private Node _root;

        private void Start()
        {
            actor = GetComponent<Actor>();
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
            
            object[] parameters = {this};
            return (Node)Activator.CreateInstance(nodeType, parameters);
        }
    }
}