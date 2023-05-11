using System.Collections.Generic;
using UnityEngine;

namespace Chimera.AI
{
    /// <summary>
    /// Behaviour Trees need to register to this class to be executed in a single place, avoiding Unity callbacks for each tree.
    /// </summary>
    public class BehaviourTreeRunner : MonoBehaviour
    {
        public static BehaviourTreeRunner Instance { get; private set; }

        public const int InvalidId = -1;
        private static int _treeIdCounter = 0;
        private Dictionary<int, BehaviourTree> _trees = new();

        #region MonoBehaviour

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Behaviour Tree Runner already exists in this scene!");
                return;
            }
            
            Instance = this;
        }
        
        /// <summary>
        /// Evaluates all registered behaviour trees.
        /// </summary>
        private void Update()
        {
            foreach (var treeEntry in _trees)
            {
                if (treeEntry.Value != null)
                {
                    treeEntry.Value.Evaluate();
                }
            }
        }
        
        #endregion
        
        /// <summary>
        /// Adds a behaviour tree to the runner. Returns a unique id.
        /// </summary>
        public int RegisterBehaviourTree(BehaviourTree tree)
        {
            _treeIdCounter++;
            _trees.Add(_treeIdCounter, tree);
            return _treeIdCounter;
        }

        /// <summary>
        /// Removes a behaviour tree from the runner, by its unique id.
        /// </summary>
        public void UnregisterBehaviourTree(int treeId)
        {
            if (_trees.ContainsKey(treeId))
            {
                _trees.Remove(treeId);
            }
        }
    }
}