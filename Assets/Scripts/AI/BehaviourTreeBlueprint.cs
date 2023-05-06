using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chimera.AI
{
    [Serializable]
    public class NodeBlueprint
    {
        [TypeDropdown(typeof(Node))]
        public string type;
        public List<NodeBlueprint> children;
    }

    [CreateAssetMenu(menuName = "Chimera/Actors/AI/Behaviour Tree Blueprint", fileName = "BehaviourTreeBlueprint")]
    public class BehaviourTreeBlueprint : ScriptableObject
    {
        public NodeBlueprint root;
    }
}