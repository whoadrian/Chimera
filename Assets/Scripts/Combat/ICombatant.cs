
namespace Chimera.Combat
{
    public enum Faction
    {
        Red,
        Green,
        Blue
    }
    
    /// <summary>
    /// Interface implemented by any actor that participates in combat. Combat-related settings and callbacks.
    /// </summary>
    public interface ICombatant
    {
        public Faction Faction { get; }
        
        public float CurrentHealth { get; }
        public float MaxHealth { get; }

        public void DealDamage(float amount);
        public void Heal(float amount);
    }
}