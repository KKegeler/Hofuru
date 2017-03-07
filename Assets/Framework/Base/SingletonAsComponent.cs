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
        private static T _instance;
        protected bool _alive = true;
        #endregion

        #region Properties
        protected static SingletonAsComponent<T> _Instance
        {
            get
            {
                if (!_instance)
                {
                    T[] managers = FindObjectsOfType(typeof(T)) as T[];
                    if (managers != null)
                    {
                        if (managers.Length == 1)
                        {
                            _instance = managers[0];
                            return _instance;
                        }
                        else if (managers.Length > 1)
                        {
                            CustomLogger.LogWarningFormat("More than one instance of {0} exists!",
                                _instance.name);
                            for (int i = 0; i < managers.Length; ++i)
                            {
                                T manager = managers[i];
                                Destroy(manager.gameObject);
                            }
                        }
                    }

                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    _instance = obj.GetComponent<T>();
                    DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
        }

        public static bool IsAlive
        {
            get
            {
                if (_instance == null)
                    return false;

                return _instance._alive;
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