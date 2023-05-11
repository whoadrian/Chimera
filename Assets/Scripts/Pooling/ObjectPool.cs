using System.Collections.Generic;
using UnityEngine;

namespace Chimera.Pooling
{
    /// <summary>
    /// Singleton pattern for object pooling. Use as ObjectPool.Instance.
    /// Works with the IPoolable interface to spawn and re-use poolable objects such as fx, projectiles, etc.
    /// Clears the objects after a period of time of them being unused.
    /// Parents the pooled objects by prefab type.
    /// </summary>
    public class ObjectPool : MonoBehaviour
    {
        // Singleton pattern
        private static ObjectPool _instance;
        public static ObjectPool Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = GameObject.Find("ObjectPool");
                    if (go != null)
                    {
                        _instance = go.GetComponent<ObjectPool>();
                    }
                    else
                    {
                        go = new GameObject("ObjectPool");
                        go.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                        _instance = go.AddComponent<ObjectPool>();
                    }
                }

                return _instance;
            }
        }

        // Data for pooled objects
        private class PoolableData
        {
            // Time at which it was spawned
            public float timestamp;
            // Interface
            public IPoolable poolable;
        }
        
        // Data for a pool of objects
        private class PoolData
        {
            // Parent of pool
            public Transform parent;
            // Objects
            public List<PoolableData> pool;
        }
        
        // Timer to destroy objects after being unused for a period of time
        private const float _destroyAfterSec = 20;
        
        // Pools
        private Dictionary<string, PoolData> _pools = new();

        /// <summary>
        /// Pass in a prefab reference to get a pooled instance. Will create one if there is none. Prefab needs an IPoolable component.
        /// </summary>
        public IPoolable GetObject(GameObject prefab)
        {
            // Check if existing prefab type
            if (_pools.TryGetValue(prefab.name, out var poolData))
            {
                // Check for inactive instances
                foreach (var p in poolData.pool)
                {
                    if (!p.poolable.IsActive())
                    {
                        // Reset timestamp
                        p.timestamp = Time.realtimeSinceStartup;
                        return p.poolable;
                    }
                }
            }
            else // Prefab type not present
            {
                // Create new pool
                var newPoolData = new PoolData();
                newPoolData.parent = new GameObject(prefab.name).transform;
                newPoolData.parent.SetParent(Instance.transform);
                newPoolData.parent.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                newPoolData.pool = new();
                _pools.Add(prefab.name, newPoolData);
            }

            // We don't have an instance yet, create it
            var newObject = GameObject.Instantiate(prefab, _pools[prefab.name].parent);
            var newPoolable = newObject.GetComponent<IPoolable>();

            // Check for IPoolable interface
            if (newPoolable == null)
            {
                Debug.LogError($"Prefab {prefab.name} does not have an IPoolable component!");
                return null;
            }

            // Add to pool and return
            _pools[prefab.name].pool.Add(new PoolableData() {poolable = newPoolable, timestamp = Time.realtimeSinceStartup});
            return newPoolable;
        }

        private void Update()
        {
            // Destroy inactive pooled objects after a number of seconds of not being used
            foreach (var poolData in _pools)
            {
                for (int i = poolData.Value.pool.Count - 1; i >= 0; --i)
                {
                    var poolable = poolData.Value.pool[i].poolable;
                    if (poolable.IsActive())
                    {
                        continue;
                    }
                    
                    var timeSinceSpawn = Time.realtimeSinceStartup - poolData.Value.pool[i].timestamp;
                    if (timeSinceSpawn < _destroyAfterSec)
                    {
                        continue;
                    }
                    
                    poolData.Value.pool.RemoveAt(i);
                    var behaviour = poolable as MonoBehaviour;
                    GameObject.Destroy(behaviour.gameObject);
                }
            }
        }
    }
}