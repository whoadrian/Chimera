
namespace Chimera
{
    /// <summary>
    /// Implement this for any component that can be selected by the player, via the PlayerControl class.
    /// </summary>
    public interface ISelectable
    {
        public bool Selected { get; set; }
    }
}