
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : Managers<EventManager>
{
    public OneTimeAction InvokeCalReset = new OneTimeAction();
    public OneTimeAction InvokeCalculate = new OneTimeAction();
    public OneTimeAction InvokeDrop = new OneTimeAction();
    public OneTimeAction InvokeBlast = new OneTimeAction();
    public OneTimeAction InvokeOrganize = new OneTimeAction();


    //포커싱 후 폭발할때 피 포커싱 타일은 바로 해제하기 위한 액션
    public OneTimeAction InvokeFocusBlast = new OneTimeAction();
    public OneTimeAction InvokeReMove = new OneTimeAction();

    //타일 전체에 대한 이벤트
    [HideInInspector]
    public SimpleEvent InvokeSpawnTile = new SimpleEvent();
    [HideInInspector]
    public SimpleEvent<Tile, Vector2> OnDisabledTile = new SimpleEvent<Tile, Vector2>();
    [HideInInspector]
    public SimpleEvent<Tile, Vector2> OnBlastTile = new SimpleEvent<Tile, Vector2>();
    [HideInInspector]
    public SimpleEvent<Tile, TileDestroyer, Vector2> OnBlastTileByBomb = new SimpleEvent<Tile, TileDestroyer, Vector2>();
    [HideInInspector]
    public SimpleEvent<Tile, Vector2> OnBombActived = new SimpleEvent<Tile, Vector2>();
    [HideInInspector]
    public SimpleEvent<Tile, Vector2> OnOrganized = new SimpleEvent<Tile, Vector2>();
    [HideInInspector]
    public SimpleEvent<IActiveObject, Vector2> OnSpawnedActiveOjbect = new SimpleEvent<IActiveObject, Vector2>();

    public int movingTiles;
    public int dropTiles;
    public int moveTestTiles;
    public bool readyToFocus;
    public bool NeedTestCalculation;
    public int activeDestroyer;

    public void MoveTest()
    {
        StartCoroutine(MoveTestWait());
    }

    private void FixedUpdate() {
        //포커싱 될 준비가 끝나면 포커싱 대기
        if (readyToFocus == true)
        {
            return;
        }

        GameSpeedManager.Instance.StartSpeedIncrease();
        
        InvokeBlast.Invoke();

        if (activeDestroyer <= 0 && dropTiles <= 0)
        {   
            
            InvokeSpawnTile.Invoke();   

            if (dropTiles <= 0)
            {
                InvokeDrop.Invoke();
            }


            if (movingTiles <= 0)
            {
                InvokeCalReset.Invoke();
                InvokeCalculate.Invoke();

                if (InvokeBlast.Count <= 0)
                {
                    GameSpeedManager.Instance.StopSpeedIncrease();
                    readyToFocus = true;
                }
            }
        }
    }
        

    IEnumerator MoveTestWait()
    {
        while (moveTestTiles > 0)
        {
            yield return null;
        }

        InvokeCalReset.Invoke();
        InvokeCalculate.Invoke();

        // 폭발이 필요 없는 경우 되돌리기
        if (InvokeBlast.Count <= 0)
        {
            // Debug.Log("폭발 필요 없음");
            InvokeFocusBlast.Clear();
            InvokeReMove.Invoke();
        }
        else
        {
            // Debug.Log("폭발 필요");
            InvokeReMove.Clear();
            InvokeFocusBlast.Invoke();
            readyToFocus = false;
        }
    }

    

}
