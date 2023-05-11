using Chimera.Combat;
using UnityEngine;

namespace Chimera
{
    /// <summary>
    /// General game data config.
    /// </summary>
    [CreateAssetMenu(menuName = "Chimera/Game Config", fileName = "GameConfig")]
    public class GameConfig : ScriptableObject
    {
        // Global player current level, stored in PlayerPrefs
        public static int Level
        {
            get => PlayerPrefs.HasKey(LevelPref) ? PlayerPrefs.GetInt(LevelPref) : 0;
            set => PlayerPrefs.SetInt(LevelPref, value);
        }
        private const string LevelPref = "Level";

        // Player faction
        public Faction playerFaction = Faction.Red;
    }
}