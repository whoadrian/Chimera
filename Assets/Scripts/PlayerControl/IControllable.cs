using UnityEngine;

namespace Chimera
{
    /// <summary>
    /// Implement this for any component that can be controlled by the player, via the PlayerControl class.
    /// </summary>
    public interface IControllable
    {
        public void OnMoveCommand(Vector3 destination);
        public void OnAttackCommand(Actor actor);
    }
}