using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    [HideInInspector]
    public UnityEvent OnCalReset = new UnityEvent();

    [HideInInspector]
    public UnityEvent OnCalculate = new UnityEvent();
    void Awake(){   
        //싱글톤
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }
    

}
