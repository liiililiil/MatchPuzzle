using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    public ActionStack OnCalReset = new ActionStack();
    public ActionStack OnCalculate = new ActionStack();
    public ActionStack OnDrop = new ActionStack();
    public ActionStack OnDispose = new ActionStack();

    //타일 전체에 대한 이벤트트
    [HideInInspector]
    public UnityEvent OnSpawnTile;
    [HideInInspector]
    public UnityEvent<ITile, Vector2> OnDisabledTile;
    [HideInInspector]
    public UnityEvent<ITile, Vector2> OnBlastedTile;
    [HideInInspector]
    public UnityEvent<ITile, ITileDestroyer, Vector2> OnBlastedTileByBomb;
    [HideInInspector]
    public UnityEvent<ITile, Vector2> OnBombActived;
    [HideInInspector]
    public UnityEvent<ITile, Vector2> OnOrganized;
    [HideInInspector]
    public UnityEvent<ITile, Vector2> OnSpawnedTile;

    public int movingTiles;
    private float time;



    void Awake()
    {
        //싱글톤
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    private void LateUpdate()
    {
        time += Time.deltaTime * (Utils.MOVEMENT_SPEED * 2);
                    OnDrop.Invoke();
            OnSpawnTile.Invoke();
        
            if (OnDrop.Count() <= 0)
            {
                OnCalReset.Invoke();
                OnCalculate.Invoke();

            }

        if (time >= 1)
        {



            time -= 1;


        }
    }
    

}
