using UnityEngine;
using System.Collections.Generic;
using Framework.Log;

namespace Framework
{
    namespace Pool
    {
        /// <summary>
        /// Creates pools in the Hierarchy
        /// </summary>
        public class PoolObjectManager : MonoBehaviour
        {
            #region Variables
            private static PoolObjectManager _instance;

            [SerializeField]
            private List<GameObject> _poolObjects = new List<GameObject>();
            [SerializeField]
            private List<int> _poolSizes = new List<int>();
            #endregion

            #region Properties
            public static PoolObjectManager Instance
            {
                get { return _instance; }
            }

            public List<GameObject> PoolObjects
            {
                get { return _poolObjects; }
            }
            #endregion

            private void Awake()
            {
                if (!_instance)
                    _instance = this;
                else if (_instance != this)
                    Destroy(gameObject);
            }

            void Start()
            {
                // Iterate the lists and create pools
                if (_poolObjects.Count != _poolSizes.Count)
                    CustomLogger.LogError("PoolObjectManager: Counts don't match!\n");
                else
                    for (int i = 0; i < _poolObjects.Count; ++i)
                        PoolManager.Instance.CreatePool(_poolObjects[i], _poolSizes[i]);
            }

        }
    }
}