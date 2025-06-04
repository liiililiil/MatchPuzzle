using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    public Action OnCalReset;
    public Action OnCalculate;

    //타일 전체에 대한 액션
    public Action<ITile, Vector2> OnDisableTile;
    public Action<ITile, Vector2> OnBlastTile;
    public Action<ITile, ITileDestroyer, Vector2> OnBlastTileByBomb;
    public Action<ITile, Vector2> OnBombActive;
    public Action<ITile, Vector2> OnOrganized;
    public Action<ITile, Vector2> OnSpawnTile;



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
