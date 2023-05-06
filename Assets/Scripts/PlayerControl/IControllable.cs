using UnityEngine;

namespace Chimera
{
    public interface IControllable
    {
        public void OnMoveCommand(Vector3 destination);
        public void OnAttackCommand(Actor actor);
    }
}