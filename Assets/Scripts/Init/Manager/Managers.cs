using UnityEngine;

public abstract class Managers<T> : MonoBehaviour where T : Managers<T>
{
    public static T Instance;
    protected void Awake()
    {
        if(Instance == this) return;
        // 싱글톤
        if (Instance == null)
        {
            Instance = (T)this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != (T)this)
        {
            Destroy(gameObject);
        }

        OnAwake();
    }

    protected virtual void OnAwake()
    {
        
    }

}
