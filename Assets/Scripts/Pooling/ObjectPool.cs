using System.Collections.Generic;
using UnityEngine;

namespace Chimera.Pooling
{
    public class ObjectPool : MonoBehaviour
    {
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

        private class PoolableData
        {
            public float timestamp;
            public IPoolable poolable;
        }
        
        private class PoolData
        {
            public Transform parent;
            public List<PoolableData> pool;
        }
        
        private static float _destroyAfterSec = 20;
        private Dictionary<string, PoolData> _pools = new();

        public IPoolable GetObject(GameObject prefab)
        {
            if (_pools.TryGetValue(prefab.name, out var poolData))
            {
                foreach (var p in poolData.pool)
                {
                    if (!p.poolable.IsActive())
                    {
                        p.timestamp = Time.realtimeSinceStartup;
                        return p.poolable;
                    }
                }
            }
            else
            {
                var newPoolData = new PoolData();
                newPoolData.parent = new GameObject(prefab.name).transform;
                newPoolData.parent.SetParent(Instance.transform);
                newPoolData.parent.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                newPoolData.pool = new();
                _pools.Add(prefab.name, newPoolData);
            }

            var newObject = GameObject.Instantiate(prefab, _pools[prefab.name].parent);
            var newPoolable = newObject.GetComponent<IPoolable>();

            if (newPoolable == null)
            {
                Debug.LogError($"Prefab {prefab.name} does not have an IPoolable component!");
                return null;
            }

            _pools[prefab.name].pool.Add(new PoolableData() {poolable = newPoolable, timestamp = Time.realtimeSinceStartup});
            return newPoolable;
        }

        private void Update()
        {
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