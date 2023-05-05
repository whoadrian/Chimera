using UnityEngine;

namespace Chimera
{
    public class Character : MonoBehaviour
    {
        public enum Faction
        {
            Red,
            Green,
            Blue
        }
        
        public CharacterConfig config;
    }
}