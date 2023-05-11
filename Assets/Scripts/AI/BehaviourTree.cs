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
        private Dictionary<string, object> _nodesContext = new();
        private KeyValuePair<string, object> _commandContext = new(string.Empty, null);
        private int _id = BehaviourTreeRunner.InvalidId;

        private void Start()
        {
            actor = GetComponent<Actor>();
            _root = BuildTree(blueprint.root, this);

            if (BehaviourTreeRunner.Instance)
            {
                _id = BehaviourTreeRunner.Instance.RegisterBehaviourTree(this);
            }
        }

        private void OnDestroy()
        {
            if (_id != BehaviourTreeRunner.InvalidId)
            {
                BehaviourTreeRunner.Instance.UnregisterBehaviourTree(_id);
            }
        }

        public void Evaluate()
        {
            SetNodesContext(Context.Nodes.DestinationKey, null);
            
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

        public void SetCommandContext(string key, object value)
        {
            _commandContext = new KeyValuePair<string, object>(key, value);
        }

        public object GetCommandContext(string key)
        {
            return _commandContext.Key == key ? _commandContext.Value : null;
        }
        
        public void SetNodesContext(string key, object value)
        {
            _nodesContext[key] = value;
        }

        public object GetNodesContext(string key)
        {
            if (_nodesContext.TryGetValue(key, out var value))
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
            SetCommandContext(Context.Commands.MoveToCommandKey, destination);
        }

        public void OnAttackCommand(Actor actor)
        {
            SetCommandContext(Context.Commands.AttackCommandKey, actor.transform);
        }

        #endregion
    }
}