using UnityEngine;

namespace Chimera.Pooling
{
    /// <summary>
    /// Implement this for any object that can be pooled via the ObjectPool class.
    /// </summary>
    public interface IPoolable
    {
        public bool IsActive();
        public void Activate(Vector3 position, Quaternion rotation);
    }
}