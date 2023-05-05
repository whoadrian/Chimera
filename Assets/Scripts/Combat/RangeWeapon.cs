using UnityEngine;

namespace Chimera
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