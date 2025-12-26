
using System.Collections;
using UnityEngine;




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

    //스테이지 시작
    [HideInInspector]
    public SimpleEvent OnStageStart = new SimpleEvent();

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

    [HideInInspector]
    public SimpleEvent OnMovedTile = new SimpleEvent();

    public int dropTiles;
    public int moveTestTiles;
    public int activeDestroyer;

    public bool isCanFocus;
    private Coroutine moveTestCoroutine;

    [SerializeField]
    public Phase phase { get; private set; } = Phase.Drop;
    
    public void MoveTest()
    {
        if(moveTestCoroutine != null) return;

        moveTestCoroutine = StartCoroutine(MoveTestWait());
    }


    private void FixedUpdate() {
        // Debug.Log(phase);
        switch (phase)
        {
            case Phase.Drop:
                DropPhase();
                break;
            case Phase.Focus:
                FocusPhase();
                break;
            case Phase.FocusWait:
                FocusWaitPhase();
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
            isCanFocus = true;
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

        if(Utils.IsDown())
        {
            phase = Phase.FocusWait;
            return;
        }
    }

    private void FocusWaitPhase()
    {
        if(!Utils.IsDown() && moveTestCoroutine == null)
        {
            phase = Phase.Blast;
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
            // 되돌리기
            InvokeFocusBlast.Clear();
            InvokeReMove.Invoke();
        }
        else
        {
            // 폭발
            Instance.OnMovedTile.Invoke();
            InvokeReMove.Clear();
            InvokeFocusBlast.Invoke();
        }

        while (moveTestTiles > 0)
        {
            yield return null;
        }
        
        CourtineStop(ref moveTestCoroutine);
    }
    private void CourtineStop(ref Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    

}
