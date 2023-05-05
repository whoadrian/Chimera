using UnityEngine;

namespace Chimera.Pooling
{
    [RequireComponent(typeof(ParticleSystem))]
    public class PoolableParticleSystem : MonoBehaviour, IPoolable
    {
        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }
        
        public bool IsActive()
        {
            return _particleSystem.IsAlive();
        }

        public void Activate(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);
            _particleSystem.Play(true);
        }
    }
}