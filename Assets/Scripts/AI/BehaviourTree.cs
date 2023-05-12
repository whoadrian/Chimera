using System;
using System.Collections.Generic;
using Chimera.AI.Utilities;
using UnityEngine;

namespace Chimera.AI
{
    /// <summary>
    /// Behaviour tree for actors. Constructs a behaviour tree from a BehaviourTreeBlueprint and registers it to
    /// the BehaviourTreeRunner.
    /// </summary>
    [RequireComponent(typeof(Actor))]
    public class BehaviourTree : MonoBehaviour, IControllable
    {
        // The blueprint out of which this tree is constructed
        public BehaviourTreeBlueprint blueprint;

        // Our main actor for this tree
        [HideInInspector] public Actor actor;
        
        // Root node of tree
        private Node _root;
        
        // Context data for use within nodes
        private Dictionary<Context.NodeKey, object> _nodesContext = new();
        
        // Context data for player-issued commands
        private KeyValuePair<Context.CommandKey, object> _commandContext = new(Context.CommandKey.None, null);
        
        // Tree unique id
        private int _id = BehaviourTreeRunner.InvalidId;

        #region MonoBehaviour
        
        private void Start()
        {
            actor = GetComponent<Actor>();
            
            // Build behaviour tree
            _root = BuildTree(blueprint.root, this);

            // Register to BehaviourTreeRunner instance
            if (BehaviourTreeRunner.Instance)
            {
                _id = BehaviourTreeRunner.Instance.RegisterBehaviourTree(this);
            }
        }

        private void OnDestroy()
        {
            // Unregister from BehaviourTreeRunner instance
            if (_id != BehaviourTreeRunner.InvalidId)
            {
                BehaviourTreeRunner.Instance.UnregisterBehaviourTree(_id);
            }
        }
        
        #endregion

        public void Evaluate()
        {
            { // Prepare data for evaluation
                
                // Reset destination position context
                SetNodesContext(Context.NodeKey.Destination, null);
                
                // Disable attack animation
                if (actor.config.attackAnimBool != string.Empty)
                {
                    actor.animator.SetBool(actor.config.attackAnimBool, false);
                }

                // Disable walk animation
                if (actor.config.walkAnimBool != string.Empty)
                {
                    actor.animator.SetBool(actor.config.walkAnimBool, false);
                }
            }

            // Evaluate tree
            _root?.Evaluate();
        }

        /// <summary>
        /// Set data in the player-issued command context. This will replace any existing command data.
        /// </summary>
        public void SetCommandContext(Context.CommandKey key, object value)
        {
            _commandContext = new KeyValuePair<Context.CommandKey, object>(key, value);
        }

        /// <summary>
        /// Get player-issued command data.
        /// </summary>
        public object GetCommandContext(Context.CommandKey key)
        {
            return _commandContext.Key == key ? _commandContext.Value : null;
        }
        
        /// <summary>
        /// Set data in the nodes context, to be used by other nodes. Will only replace any data with the same key.
        /// </summary>
        public void SetNodesContext(Context.NodeKey key, object value)
        {
            _nodesContext[key] = value;
        }

        /// <summary>
        /// Get data from the nodes context, if any. Returns null if not existing.
        /// </summary>
        public object GetNodesContext(Context.NodeKey key)
        {
            if (_nodesContext.TryGetValue(key, out var value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// Builds the tree recursively from the BehaviourTreeBlueprint
        /// </summary>
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

        /// <summary>
        /// Builds a node from the BehaviourTreeBlueprint data.
        /// </summary>
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
            // Set player-issued command context
            SetCommandContext(Context.CommandKey.MoveToCommand, destination);
        }

        public void OnAttackCommand(Actor actor)
        {
            // Set player-issued command context
            SetCommandContext(Context.CommandKey.AttackCommand, actor.transform);
        }

        #endregion
    }
}