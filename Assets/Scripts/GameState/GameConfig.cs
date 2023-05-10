using Chimera.Combat;
using UnityEngine;

namespace Chimera
{
    [CreateAssetMenu(menuName = "Chimera/Game Config", fileName = "GameConfig")]
    public class GameConfig : ScriptableObject
    {
        private static string LevelPref = "Level";
        public static int Level
        {
            get => PlayerPrefs.HasKey(LevelPref) ? PlayerPrefs.GetInt(LevelPref) : 0;
            set => PlayerPrefs.SetInt(LevelPref, value);
        }

        public Faction playerFaction = Faction.Red;
    }
}