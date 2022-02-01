using UnityEngine;
/// <summary>
/// Standard generic Singleton for Unity
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class MonoSingleton<T> : MonoBehaviour where T: MonoSingleton<T>
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
                Debug.Log(typeof(T).ToString() + " is NULL");
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this as T;
    }
}
