using UnityEngine;

namespace Chimera.Pooling
{
    /// <summary>
    /// Component for particle systems that work with the ObjectPool class, and IPoolable interface
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public class PoolableParticleSystem : MonoBehaviour, IPoolable
    {
        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        #region IPoolable
        
        public bool IsActive()
        {
            return _particleSystem.IsAlive();
        }

        public void Activate(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);
            _particleSystem.Play(true);
        }
        
        #endregion
    }
}