using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SestiKupi.Core
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        protected static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.Log("Looking for singleton of type " + typeof(T));
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        Debug.Log("Couldn't find singleton of " + typeof(T) + "Creating...");
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        instance = obj.AddComponent<T>();
                    }
                }
                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogWarning("Dublicate singletof of " + typeof(T) + " Destroying...");
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
    }
}
