using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T> //Initialize singletons
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = (T)this;
        else
            Destroy(gameObject);
    }
}