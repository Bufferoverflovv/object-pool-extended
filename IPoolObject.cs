using System.Collections;
using UnityEngine;

namespace Buffer.ObjectPooling
{
    public interface IPoolObject
    {
        /// <summary>
        /// The prefab used as a key to the pool.
        /// </summary>
        GameObject Prefab { get; set; }
        
        /// <summary>
        /// Time to wait before returning to pool.
        /// </summary>
        int WaitTime { get; }
        
        /// <summary>
        /// Is the object initialized?
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// Called by the pool when the object is first created to perform any necessary initialization.
        /// </summary>
        void Initialize(params object[] args);

        /// <summary>
        /// Called by pool when object instantiated.
        /// </summary>
        void OnCreatedInPool();

        /// <summary>
        /// Called by pool before you get a pool's object. Useful for state resetting.
        /// </summary>
        void OnGettingFromPool();
        
        /// <summary>
        /// Called by pool when you return a pool's object. Useful for state resetting.
        /// </summary>
        void OnReturnToPool();
        
        /// <summary>
        /// Return the object back to the pool after some time
        /// </summary>
        /// <returns></returns>
        IEnumerator ReturnToPoolAfterTime();
    }
}
