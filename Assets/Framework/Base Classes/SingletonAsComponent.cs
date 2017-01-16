using UnityEngine;

namespace Framework
{
    /// <summary>
    /// Basisklasse für Singletons
    /// </summary>
    /// <typeparam name="T">Abgeleitete Klasse</typeparam>
    public class SingletonAsComponent<T> : MonoBehaviour where T : SingletonAsComponent<T>
    {
        #region Variablen
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
                    T[] managers = FindObjectOfType(typeof(T)) as T[];
                    if (managers != null)
                    {
                        if (managers.Length == 1)
                        {
                            __Instance = managers[0];
                            return __Instance;
                        }
                        else if (managers.Length > 1)
                        {
                            Debug.LogWarningFormat("More than one instance of {0} exists!",
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

        void OnDestroy()
        {
            _alive = false;
        }

        void OnApplicationQuit()
        {
            _alive = false;
        }

    }
}