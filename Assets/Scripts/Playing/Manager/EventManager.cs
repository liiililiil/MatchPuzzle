using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    public OneTimeAction InvokeCalReset = new OneTimeAction();
    public OneTimeAction InvokeCalculate = new OneTimeAction();
    public OneTimeAction InvokeDrop = new OneTimeAction();
    public OneTimeAction InvokeBlast = new OneTimeAction();
    public OneTimeAction InvokeOrganize = new OneTimeAction();
    public OneTimeAction InvokeDestroyerMove = new OneTimeAction();

    //타일 전체에 대한 이벤트트
    [HideInInspector]
    public UnityEvent InvokeSpawnTile;
    [HideInInspector]
    public UnityEvent<Tile, Vector2> OnDisabledTile;
    [HideInInspector]
    public UnityEvent<Tile, Vector2> OnBlastTile;
    [HideInInspector]
    public UnityEvent<Tile, TileDestroyer, Vector2> OnBlastTileByBomb;
    [HideInInspector]
    public UnityEvent<Tile, Vector2> OnBombActived;
    [HideInInspector]
    public UnityEvent<Tile, Vector2> OnOrganized;
    [HideInInspector]
    public UnityEvent<IActiveObject, Vector2> OnSpawnedActiveOjbect;

    public int movingTiles;
    public int activeDestroyer;
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

        // OnDisabledTile.AddListener(deBugTest);
    }
    private void deBugTest(Tile tile, Vector2 vector2)
    {
        SpawnManager.Instance.SpawnObject(DestroyerType.Straight, vector2, Quaternion.Euler(0, 0, 1), tile);
    }

    //생명 주기

    private void LateUpdate()
    {
        time += Time.deltaTime * (Utils.MOVEMENT_SPEED * 2);

        if (time >= 1)
        {
            InvokeBlast.Invoke();

            if (movingTiles <= 0 && activeDestroyer <= 0)
            {
                InvokeCalReset.Invoke();
                InvokeCalculate.Invoke();
            }

            InvokeDestroyerMove.Invoke();
            InvokeDrop.Invoke();

            if (activeDestroyer <= 0) InvokeSpawnTile.Invoke();
            time -= 1;
        }
    }
    

}
