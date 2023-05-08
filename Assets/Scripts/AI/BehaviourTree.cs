using System;
using System.Collections.Generic;
using Chimera.AI.Utilities;
using UnityEngine;

namespace Chimera.AI
{
    [RequireComponent(typeof(Actor))]
    public class BehaviourTree : MonoBehaviour, IControllable
    {
        public BehaviourTreeBlueprint blueprint;

        [HideInInspector] public Actor actor;
        private Node _root;
        private Dictionary<string, object> _context = new();

        private void Start()
        {
            actor = GetComponent<Actor>();
            _root = BuildTree(blueprint.root, this);
        }

        private void Update()
        {
            if (actor.config.attackAnimBool != string.Empty)
            {
                actor.animator.SetBool(actor.config.attackAnimBool, false);
            }

            if (actor.config.walkAnimBool != string.Empty)
            {
                actor.animator.SetBool(actor.config.walkAnimBool, false);
            }

            _root?.Evaluate();
        }
        
        public void SetContext(string key, object value)
        {
            _context[key] = value;
        }

        public object GetContext(string key)
        {
            if (_context.TryGetValue(key, out var value))
            {
                return value;
            }

            return null;
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

        #region IControllable

        public void OnMoveCommand(Vector3 destination)
        {
            SetContext(Context.MoveToCommandKey, destination);
        }

        public void OnAttackCommand(Actor actor)
        {
            SetContext(Context.AttackCommandKey, actor.transform);
        }

        #endregion
    }
}