using UnityEngine;
using UnityEngine.Pool;

namespace Buffer.ObjectPooling
{
    public static class Pool
    {
        /// <summary>
        /// Create a new object pool
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="container"></param>
        /// <param name="initialSize"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        public static ObjectPool<GameObject> Create(GameObject prefab, Transform container, int initialSize, int maxSize)
        {
            var pool = new ObjectPool<GameObject>(
                () => OnCreate(prefab, container),
                (obj) => OnGet(obj),
                (obj) => OnRelease(obj),
                (obj) => OnDestroy(obj),
                false, initialSize, maxSize);

            return pool;
        }

        /// <summary>
        /// Called when an object is created in the pool
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        private static GameObject OnCreate(GameObject prefab, Transform container = null)
        {
            GameObject newGameObject = Object.Instantiate(prefab, container);

            IPoolObject poolObject = newGameObject.GetComponent<IPoolObject>();
            if (poolObject != null)
                poolObject.OnCreatedInPool();

            return newGameObject;
        }

        /// <summary>
        /// Called when an object is retrieved from the pool
        /// </summary>
        /// <param name="gameObject"></param>
        private static void OnGet(GameObject gameObject)
        {
            gameObject.SetActive(true);

            IPoolObject poolObject = gameObject.GetComponent<IPoolObject>();
            if (poolObject != null && poolObject.IsInitialized)
                poolObject.OnGettingFromPool();
        }

        /// <summary>
        /// Called when an object is returned to the pool
        /// </summary>
        /// <param name="gameObject"></param>
        private static void OnRelease(GameObject gameObject)
        {
            gameObject.SetActive(false);

            IPoolObject poolObject = gameObject.GetComponent<IPoolObject>();
            if (poolObject != null)
                poolObject.OnReturnToPool();
        }

        /// <summary>
        /// Called when an object is destroyed
        /// </summary>
        /// <param name="gameObject"></param>
        private static void OnDestroy(GameObject gameObject)
        {
            Object.Destroy(gameObject);
        }
    }
}

