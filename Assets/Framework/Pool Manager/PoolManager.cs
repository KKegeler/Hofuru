using UnityEngine;
using System.Collections.Generic;
using Framework.Log;

namespace Framework
{
    namespace Pool
    {
        /// <summary>
        /// Manages pools of reusable GameObjects
        /// </summary>
        public class PoolManager : SingletonAsComponent<PoolManager>
        {
            #region Variables
            private Dictionary<int, LinkedList<ObjectInstance>> _pool =
                new Dictionary<int, LinkedList<ObjectInstance>>();
            private Dictionary<int, int[]> _activeObjects =
                new Dictionary<int, int[]>();
            private Transform _poolHolderTransform;
            #endregion

            #region Properties
            public static PoolManager Instance
            {
                get { return (PoolManager)_Instance; }
            }

            public Dictionary<int, int[]> ActiveObjects
            {
                get { return _activeObjects; }
            }
            #endregion

            /// <summary>
            /// Creates a new pool for a prefab
            /// </summary>
            /// <param name="prefab">Prefab</param>
            /// <param name="size">Pool size</param>
            public void CreatePool(GameObject prefab, int size)
            {
                int poolKey = prefab.GetInstanceID();

                if (!_pool.ContainsKey(poolKey))
                {
                    _activeObjects.Add(poolKey, new int[2]);
                    _pool.Add(poolKey, new LinkedList<ObjectInstance>());

#if UNITY_EDITOR
                    // Create a GO as parent of the objects
                    GameObject poolHolder = new GameObject(
                        string.Concat(prefab.name, " Pool"));
                    _poolHolderTransform = poolHolder.transform;
                    _poolHolderTransform.SetParent(PoolObjectManager.Instance.gameObject.transform);
#endif

                    // Instatiate pool objects         
                    for (int i = 0; i < size; ++i)
                        InstantiateObjectInstance(prefab, poolKey);
                }
                else
                {
                    CustomLogger.LogWarningFormat("{0} is already in the pool!\n",
                        prefab.name);
                }
            }

            /// <summary>
            /// Reuse objects from pool
            /// </summary>
            /// <param name="prefab">Prefab</param>
            /// <param name="position">New position</param>
            /// <param name="rotation">New rotation</param>
            public GameObject ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
            {
                int poolKey = prefab.GetInstanceID();

                ObjectInstance obj = null;

                if (_pool.ContainsKey(poolKey))
                {
                    obj = _pool[poolKey].First.Value;
                    _pool[poolKey].RemoveFirst();
                    _pool[poolKey].AddLast(obj);

                    obj.Reuse(position, rotation);
                }
                else
                {
                    CustomLogger.LogWarningFormat("{0} is not in the pool!\n",
                        prefab.name);
                }

                return obj.Object;
            }

            /// <summary>
            /// Reuse objects from pool. If all objects are active, add a new one
            /// </summary>
            /// <param name="prefab">Prefab</param>
            /// <param name="position">New position</param>
            /// <param name="rotation">New rotation</param>
            public GameObject ReuseObject2(GameObject prefab, Vector3 position, Quaternion rotation)
            {
                int poolKey = prefab.GetInstanceID();

                if (!_pool[poolKey].First.Value.CanReuse)
                {
                    CustomLogger.LogWarningFormat("{0} doesn't implement IPoolObject!\n",
                        _pool[poolKey].First.Value.Object.name);
                    return null;
                }

                ObjectInstance obj = null;

                if (_activeObjects.ContainsKey(poolKey))
                {
                    int[] array = _activeObjects[poolKey];

                    if (array[0] <= array[1])
                        InstantiateObjectInstance(prefab, poolKey);

                    obj = _pool[poolKey].First.Value;
                    _pool[poolKey].RemoveFirst();
                    _pool[poolKey].AddLast(obj);

                    obj.Reuse(position, rotation);
                }
                else
                {
                    CustomLogger.LogWarningFormat("{0} is not in the pool!\n",
                        prefab.name);
                }

                return obj.Object;
            }

            /// <summary>
            /// Adds a new ObjectInstance to the pool
            /// </summary>
            /// <param name="prefab">Prefab</param>
            /// <param name="poolKey">InstanceID</param>
            private void InstantiateObjectInstance(GameObject prefab, int poolKey)
            {
                ++_activeObjects[poolKey][0];

#if UNITY_EDITOR
                ObjectInstance newObject = new ObjectInstance(
                    Instantiate(prefab, _poolHolderTransform));
#else
                ObjectInstance newObject = new ObjectInstance(Instantiate(prefab));
#endif

                if (newObject.CanReuse)
                    newObject.PoolObj.InstanceID = poolKey;

                _pool[poolKey].AddFirst(newObject);
            }

            /// <summary>
            /// Custom class for objects managed by the PoolManager
            /// </summary>
            private class ObjectInstance
            {
                #region Variables
                private readonly GameObject _obj;
                private readonly Transform _tf;
                private readonly IPoolObject _poolObj;
                private readonly bool _canReuse;
                #endregion

                #region Properties
                public GameObject Object
                {
                    get { return _obj; }
                }
                
                public IPoolObject PoolObj
                {
                    get { return _poolObj; }
                }

                public bool CanReuse
                {
                    get { return _canReuse; }
                }
                #endregion

                #region Constructors
                public ObjectInstance(GameObject go)
                {
                    _obj = go;
                    _tf = _obj.transform;
                    _poolObj = _obj.GetComponent<IPoolObject>();

                    _canReuse = _poolObj != null;

                    if (_canReuse)
                        _poolObj.IncreaseObjectCount(_obj);

                    _obj.SetActive(false);
                }
                #endregion

                /// <summary>
                /// Set new position/rotation and call OnObjectReuse
                /// </summary>
                /// <param name="position">New position</param>
                /// <param name="rotation">New rotation</param>
                public void Reuse(Vector3 position, Quaternion rotation)
                {
                    _obj.SetActive(true);
                    _tf.position = position;
                    _tf.rotation = rotation;

                    if (_canReuse)
                    {
                        _poolObj.IncreaseObjectCount(_obj);
                        _poolObj.OnObjectReuse();
                    }
                }

            }

            /// <summary>
            /// Call before loading a new Scene
            /// </summary>
            public void ResetPool()
            {
                _pool.Clear();
                _activeObjects.Clear();
            }

        }
    }
}