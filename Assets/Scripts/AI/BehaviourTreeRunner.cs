using System.Collections.Generic;
using UnityEngine;

namespace Chimera.AI
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        private static BehaviourTreeRunner _instance;
        public static BehaviourTreeRunner Instance
        {
            get
            {
                if (_instance == null)
                {
                    const string name = "BehaviourTreeRunner";
                    var go = GameObject.Find(name);
                    
                    if (go == null)
                    {
                        go = new GameObject(name);
                        _instance = go.AddComponent<BehaviourTreeRunner>();
                        DontDestroyOnLoad(go);
                    }
                    else
                    {
                        _instance = go.GetComponent<BehaviourTreeRunner>();
                        DontDestroyOnLoad(go);
                    }
                }
                
                return _instance;
            }
        }

        private static int _treeIdCounter = 0;
        private Dictionary<int, BehaviourTree> _trees = new();

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