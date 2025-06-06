using System;
using System.Collections;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    public ActionStack OnCalReset = new ActionStack();
    public ActionStack OnCalculate = new ActionStack();
    public ActionStack OnDrop = new ActionStack();
    public ActionStack OnDispose = new ActionStack();

    //타일 전체에 대한 액션
    public Action OnSpawnTile;
    public Action<ITile, Vector2> OnDisabledTile;
    public Action<ITile, Vector2> OnBlastedTile;
    public Action<ITile, ITileDestroyer, Vector2> OnBlastedTileByBomb;
    public Action<ITile, Vector2> OnBombActived;
    public Action<ITile, Vector2> OnOrganized;
    public Action<ITile, Vector2> OnSpawnedTile;

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

        if (time >= 1)
        {
            if (movingTiles == 0)
            {
                OnDrop.Invoke();
                
                try
                {
                    OnSpawnTile.Invoke();
                }
                catch
                {

                }

            }
        
            time -= 1;

        }
    }
    

}
