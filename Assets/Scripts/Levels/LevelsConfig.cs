using System.Collections.Generic;
using UnityEngine;

namespace Chimera
{
    /// <summary>
    /// Data container for all levels of the game.
    /// </summary>
    [CreateAssetMenu(menuName = "Chimera/Levels Config", fileName = "LevelsConfig")]
    public class LevelsConfig : ScriptableObject
    {
        // Win scene to be loaded when player has won
        public string winScene;
        
        // Lose scene to be loaded when player has lost
        public string loseScene;
        
        // All levels
        public List<string> levels;
    }
}