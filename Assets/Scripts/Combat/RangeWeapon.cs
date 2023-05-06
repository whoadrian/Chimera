using UnityEngine;

namespace Chimera.Combat
{
    public class RangeWeapon : MonoBehaviour
    {
        private float _damage;

        public void SetDamage(float damage)
        {
            _damage = damage;
        }
    }
}