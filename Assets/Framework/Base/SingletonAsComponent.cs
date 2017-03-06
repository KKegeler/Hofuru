using UnityEngine;
using Framework.Log;

namespace Framework
{
    /// <summary>
    /// Base class for Singletons.
    ///  Do not place objects of derived classes in the Hierarchy!
    /// </summary>
    /// <typeparam name="T">Derived class</typeparam>
    public abstract class SingletonAsComponent<T> : MonoBehaviour where T : SingletonAsComponent<T>
    {
        #region Variables
        private static T __Instance;
        protected bool _alive = true;
        #endregion

        #region Properties
        protected static SingletonAsComponent<T> _Instance
        {
            get
            {
                if (!__Instance)
                {
                    T[] managers = FindObjectsOfType(typeof(T)) as T[];
                    if (managers != null)
                    {
                        if (managers.Length == 1)
                        {
                            __Instance = managers[0];
                            return __Instance;
                        }
                        else if (managers.Length > 1)
                        {
                            CustomLogger.LogWarningFormat("More than one instance of {0} exists!",
                                __Instance.name);
                            for (int i = 0; i < managers.Length; ++i)
                            {
                                T manager = managers[i];
                                Destroy(manager.gameObject);
                            }
                        }
                    }

                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    __Instance = obj.GetComponent<T>();
                    DontDestroyOnLoad(__Instance.gameObject);
                }

                return __Instance;
            }
            set { __Instance = value as T; }
        }

        public static bool IsAlive
        {
            get
            {
                if (__Instance == null)
                    return false;

                return __Instance._alive;
            }
        }
        #endregion

        /// <summary>
        /// Can be used for preloading
        /// </summary>
        public void WakeUp() { }

        private void OnDestroy()
        {
            _alive = false;
        }

        private void OnApplicationQuit()
        {
            _alive = false;
        }

    }
}