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

    public SimpleEvent InvokeEnterFocusPhase = new SimpleEvent();

    [HideInInspector]
    public SimpleEvent OnMovedTile = new SimpleEvent();

    public int dropTiles;
    public int moveTestTiles;
    public int activeDestroyer;

    private bool needMoveTest = false;
    private bool forcePhase;

    private byte forceStopCaller;
    private bool ignoreForceStopCaller = false;

    [SerializeField]
    private Phase _phase = Phase.Drop;
    public Phase phase {get => _phase; private set => _phase = value; }

    public void ForcePause(out byte callBackValue)
    {
        forcePhase = true;

        byte buffer = (byte)Random.Range(byte.MinValue, byte.MaxValue+1);
        if(buffer == forceStopCaller)
        {
            unchecked{buffer++;}   
        }

        
        // Debug.Log("강제 멈춤!");

        forceStopCaller = buffer;
        callBackValue = buffer;
    }

    public void ForcePause(bool _ignoreForceStopCaller)
    {
        forcePhase = true;
        ignoreForceStopCaller = _ignoreForceStopCaller;
        Debug.Log("강제 멈춤!");

    }

    public void ForceRelease(byte caller, Phase targetPhase = Phase.none)
    {
        if(!ignoreForceStopCaller && caller != forceStopCaller)
        {
            Debug.LogWarning($"호출한 객체가 멈춤을 호출한 객체가 아닙니다! {caller} {forceStopCaller}");
            return;
        }

        if(targetPhase != Phase.none){
            phase = targetPhase;
        }
        
        // Debug.Log($"{phase}");

        ignoreForceStopCaller = false;
        forcePhase = false;
    }
    

    private void FixedUpdate() {
        // Debug.Log(phase);
        //임의로 멈춘 상태
        if(forcePhase) return;

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

            case Phase.MoveTest:
                MoveTestPhase();
                break;
        }
    }

    private void DropPhase()
    {
        GameSpeedManager.Instance.StartSpeedIncrease();

        InvokeDrop.Invoke();
        InvokeSpawnTile.Invoke();   

        if(dropTiles <= 0 && InvokeDrop.getCount() <= 0)
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

        if (InvokeBlast.getCount() <= 0)
        {
            InvokeEnterFocusPhase.Invoke();
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

        if(InputManager.Instance.isTouch)
        {
            phase = Phase.FocusWait;
            return;
        }
    }

    private void FocusWaitPhase()
    {
        if(!InputManager.Instance.isTouch)
        {
            needMoveTest = true;
            phase = Phase.MoveTest;
        }
    }

    private void MoveTestPhase()
    {
        if (moveTestTiles > 0)
        {
            // Debug.Log("MoveTestPhase 대기 중...", this);
            return;
        }

        


        if (needMoveTest)
        {
            needMoveTest = false;
            
            InvokeCalReset.Invoke();
            InvokeCalculate.Invoke();
            // Debug.Log($"invokeblast count: {InvokeBlast.getCount()}", this);

            // 폭발이 필요 없는 경우 되돌리기
            if (InvokeBlast.getCount() <= 0)
            {
                // Debug.Log("되돌리기", this);
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

        }

        if (moveTestTiles > 0)
        {
            return;
        }

        phase = Phase.Blast;
        
    }

    // private void ForcePausePhase()
    // {
    //     // 외부에서 개입되여 멈춘상태
    //     return;
    // }
        

    // IEnumerator MoveTestWait()
    // {
        
    //     while (moveTestTiles > 0)
    //     {
    //         yield return null;
    //     }

        
    //     InvokeCalReset.Invoke();
    //     InvokeCalculate.Invoke();


    //     // 폭발이 필요 없는 경우 되돌리기
    //     if (InvokeBlast.getCount() <= 0 || InvokeFocusBlast.getCount() <= 0)
    //     {
    //         // 되돌리기
    //         // Debug.Log($"되돌리기 {InvokeBlast.getCount()}", this);
    //         InvokeFocusBlast.Clear();
    //         InvokeReMove.Invoke();
    //     }
    //     else
    //     {
    //         // 폭발
    //         Instance.OnMovedTile.Invoke();
    //         InvokeReMove.Clear();
    //         InvokeFocusBlast.Invoke();
    //     }

    //     while (moveTestTiles > 0)
    //     {
    //         yield return null;
    //     }
        
    //     CourtineStop(ref moveTestCoroutine);
    // }


    

}
