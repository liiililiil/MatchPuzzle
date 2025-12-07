
using System.Collections;
using UnityEngine;
using UnityEngine.Events;


enum Phase
{
    Drop,
    Focus,
    Calculate,
    Blast,
    DestroyerWait
}

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

    public int dropTiles;
    public int moveTestTiles;
    public bool readyToFocus;
    public bool NeedTestCalculation;
    public int activeDestroyer;

    [SerializeField]
    private Phase phase = 0;
    
    public void MoveTest()
    {
        StartCoroutine(MoveTestWait());
    }

    private void FixedUpdate() {
        switch (phase)
        {
            case Phase.Drop:
                DropPhase();
                break;
            case Phase.Focus:
                FocusPhase();
                break;
            case Phase.Calculate:
                CalculatePhase();
                break;
            case Phase.Blast:
                BlastPhase();
                break;  
            case Phase.DestroyerWait:
                DestroyerWaitPhase();
                break;
        }
    }

    private void DropPhase()
    {
        GameSpeedManager.Instance.StartSpeedIncrease();

        InvokeDrop.Invoke();
        InvokeSpawnTile.Invoke();   

        if(dropTiles <= 0 && InvokeDrop.Count <= 0)
        {
            phase = Phase.Calculate;
        }
    }

    private void BlastPhase()
    {
        InvokeBlast.Invoke();
        if(activeDestroyer > 0)
        {
            phase = Phase.DestroyerWait;
        }
        else
        {
            phase = Phase.Drop;
            
        }
    }

    private void DestroyerWaitPhase()
    {
        if(activeDestroyer <= 0)
        {
            phase = Phase.Drop;
        }
        else
        {
            InvokeBlast.Invoke();
        }
    }

    private void CalculatePhase()
    {
        InvokeCalReset.Invoke();
        InvokeCalculate.Invoke();

        if (InvokeBlast.Count <= 0)
        {
            phase = Phase.Focus;
        }
        else
        {
            phase = Phase.Blast;
        }
    }

    private void FocusPhase()
    {
        GameSpeedManager.Instance.StopSpeedIncrease();
        readyToFocus = true;
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

            phase = Phase.Blast;
        }
    }

    

}
