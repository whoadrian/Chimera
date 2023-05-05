using UnityEngine;

namespace Chimera.Pooling
{
    public interface IPoolable
    {
        public bool IsActive();
        public void Activate(Vector3 position, Quaternion rotation);
    }
}