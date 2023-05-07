using System;
using UnityEngine;

namespace Chimera.AI
{
    [RequireComponent(typeof(Actor))]
    public class BehaviourTree : MonoBehaviour
    {
        public BehaviourTreeBlueprint blueprint;

        [HideInInspector] public Actor actor;
        private Node _root;

        private void Start()
        {
            actor = GetComponent<Actor>();
            _root = BuildTree(blueprint.root, this);
        }

        private void Update()
        {
            actor.animator.SetBool(actor.config.attackAnimBool, false);
            actor.animator.SetBool(actor.config.walkAnimBool, false);

            _root?.Evaluate();
        }

        private static Node BuildTree(NodeBlueprint parent, BehaviourTree tree)
        {
            var parentNode = BuildNode(parent.type, tree);

            foreach (var c in parent.children)
            {
                var childNode = BuildTree(c, tree);
                parentNode.AddChild(childNode);
            }

            return parentNode;
        }

        private static Node BuildNode(string nodeTypeName, BehaviourTree tree)
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

            object[] parameters = { tree };
            return (Node)Activator.CreateInstance(nodeType, parameters);
        }
    }
}