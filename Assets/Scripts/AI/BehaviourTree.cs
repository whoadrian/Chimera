using System;
using UnityEngine;

namespace Chimera.AI
{
    public class BehaviourTree : MonoBehaviour
    {
        public BehaviourTreeBlueprint blueprint;

        private Node _root = null;

        private void Update()
        {
            _root?.Evaluate();
        }

        void BuildTree()
        {
            
        }
    }
}