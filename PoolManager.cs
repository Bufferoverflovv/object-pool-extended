using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Buffer.ObjectPooling
{
    [Serializable]
    public class ObjectPoolData
    {
        [SerializeField]
        private string _name;
        public string Name => _name;

        [SerializeField]
        private GameObject _prefab;
        public GameObject Prefab => _prefab;

        [SerializeField]
        private Transform _container;
        public Transform Container => _container;

        [SerializeField]
        private int _initialSize;
        public int InitialSize => _initialSize;

        [SerializeField]
        private int _maxSize;
        public int MaxSize => _maxSize;

        public ObjectPool<Component> _objectPool;
    }

    public class PoolManager : MonoBehaviour
    {
        // make static instance of this
        public static PoolManager _instance { get; private set; }

        [SerializeField]
        private List<ObjectPoolData> _objectPoolDataList;

        private Dictionary<GameObject, ObjectPool<GameObject>> _objectPools;

        private void Initialize()
        {
            if (_instance != null)
                Destroy(gameObject);
            else
                _instance = this;
                DontDestroyOnLoad(gameObject);
        }

        private void Awake() 
        {
            Initialize();

            _objectPools = new Dictionary<GameObject, ObjectPool<GameObject>>();

            foreach (var poolData in _objectPoolDataList)
            {
                if (_objectPools.ContainsKey(poolData.Prefab))
                {
                    Debug.LogWarning($"Duplicate pool detected for prefab {poolData.Prefab.name}. Skipping.");
                    continue; // Skip this iteration and continue with the next
                }

                _objectPools[poolData.Prefab] = Pool.Create(poolData.Prefab, poolData.Container, poolData.InitialSize, poolData.MaxSize);
            }
        }

        /// <summary>
        /// Get an object pool by prefab
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static ObjectPool<GameObject> GetPool(GameObject prefab)
        {
            if (_instance._objectPools.TryGetValue(prefab, out var objectPool))
            {
                return objectPool;
            }
            else
            {
                throw new Exception($"Pool with prefab {prefab.name} not found");
            }
        }

        /// <summary>
        /// Get an object from the pool
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public static GameObject Get(GameObject prefab)
        {
            var pool = GetPool(prefab);
            return pool.Get();
        }

        /// <summary>
        /// Get an object from the pool and initialize it
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="initializationArgs"></param>
        /// <returns></returns>
        public static GameObject Get(GameObject prefab, params object[] initializationArgs)
        {
            var pool = GetPool(prefab);
            GameObject obj = pool.Get();

            if (obj.GetComponent<IPoolObject>() is IPoolObject poolObject && !poolObject.IsInitialized)
            {
                poolObject.Initialize(initializationArgs);
            }

            return obj;
        }

        /// <summary>
        /// Release an object back to the pool
        /// </summary>
        /// <param name="obj"></param>
        public static void Release(GameObject obj)
        {
            var pool = GetPool(obj);
            pool.Release(obj);
        }

        /// <summary>
        /// Release an object back to the pool and reset it
        /// </summary>
        /// <param name="prefab"></param>
        public static void Clear(GameObject prefab)
        {
            var pool = GetPool(prefab);
            pool.Clear();
        }


        /// <summary>
        /// Release all objects back to the pool and clear them
        /// </summary>
        public static void ClearAll()
        {
            foreach (var pool in _instance._objectPools.Values)
                pool.Clear();
        }

        /// <summary>
        /// Dispose of the pool
        /// </summary>
        /// <param name="prefab"></param>
        public static void Dispose(GameObject prefab)
        {
            var pool = GetPool(prefab);
            pool.Dispose();
        }

        /// <summary>
        /// Dispose of all pools
        /// </summary>
        public static void DisposeAll()
        {
            foreach (var pool in _instance._objectPools.Values)
                pool.Dispose();
        }

        /// <summary>
        /// Get the number of active objects in the pool
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public int CountActive(GameObject prefab)
        {
            return GetPool(prefab).CountActive;
        }

        /// <summary>
        /// Get the number of all objects in the pool
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public int CountAll(GameObject prefab)
        {
            return GetPool(prefab).CountAll;
        }

        /// <summary>
        /// Get the number of inactive objects in the pool
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public int CountInactive(GameObject prefab)
        {
            return GetPool(prefab).CountInactive;
        }

        /// <summary>
        /// Get the pool stats
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public string GetPoolStats(GameObject prefab)
        {
            var pool = GetPool(prefab);
            return $"Type: {prefab.name}, Active: {pool.CountActive}, All: {pool.CountAll}, Inactive: {pool.CountInactive}";
        }
    }
}
