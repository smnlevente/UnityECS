using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static object lockObject = new object();

    public static T Instance
    {
        get
        {
            lock (lockObject)
            {
                if (instance == null)
                {
                    instance = GameObject.FindObjectOfType<T>();
                    if (instance != null)
                    {
                        return instance;
                    }

                    GameObject singleton = new GameObject();
                    instance = singleton.AddComponent<T>();
                    singleton.name = string.Format("(singleton) {0}", typeof(T).ToString());
                    Debug.Log(singleton.name);
                }

                return instance;
            }
        }
    }
}