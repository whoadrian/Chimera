using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Chimera
{
    [CreateAssetMenu(menuName = "Chimera/Levels Config", fileName = "LevelsConfig")]
    public class LevelsConfig : ScriptableObject
    {
        public SceneAsset winScene;
        public SceneAsset loseScene;
        public List<SceneAsset> levels;
    }
}