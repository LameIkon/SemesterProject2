using UnityEngine;

[HelpURL("https://www.youtube.com/watch?v=LFOXge7Ak3E")]
public class Singleton<T> : MonoBehaviour where T : Component
{
    protected static T _instance;

    public static bool _HasInstance
    {
        get { return _instance != null; }
    }

    public static T TryGetInstance()
    {
        return _HasInstance ? _instance : null;
    }
    
    public static T _Instance 
    {
        get 
        {
            if (_instance == null) 
            {
                _instance = FindAnyObjectByType<T>();
                if (_instance == null) 
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();

                }
            }    

            return _instance;
        }
    }

    // Always call base.Awake() when you need to override the Awake function. Else the Singleton will not work properly.
    protected virtual void Awake() 
    {
        InitializeSingleton();
    }

    protected virtual void InitializeSingleton() 
    {
        if (!Application.isPlaying) 
        {
            return;
        }

        _instance = this as T;

    }


}