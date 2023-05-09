using System.Collections.Generic;
using UnityEngine;

namespace Chimera.AI
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        public static BehaviourTreeRunner Instance { get; private set; }

        public static int InvalidId = -1;
        private static int _treeIdCounter = 0;
        private Dictionary<int, BehaviourTree> _trees = new();

        private void Awake()
        {
            Instance = this;
        }
        
        public int RegisterBehaviourTree(BehaviourTree tree)
        {
            _treeIdCounter++;
            _trees.Add(_treeIdCounter, tree);
            return _treeIdCounter;
        }

        public void UnregisterBehaviourTree(int treeId)
        {
            if (_trees.ContainsKey(treeId))
            {
                _trees.Remove(treeId);
            }
        }

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
    }
}