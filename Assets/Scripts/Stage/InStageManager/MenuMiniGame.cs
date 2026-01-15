using System;
using System.Collections;
using UnityEngine;

public class MenuMiniGame : InStageManager, IInStageManager
{
    private int count = 0;

    private bool isFocusMiniGame = false;
    private Coroutine animationCoruoutine;

    [SerializeField]
    private GameObject needUp;
    protected override void OnStart()
    {
        EventManager.Instance.OnMovedTile.AddListener(OnMoveTile);
        EventManager.Instance.OnDisabledTile.AddListener(OnDisabledTile);
        Init(0, -1);
    }

    private void OnDisabledTile(Tile tile, Vector2 pos)
    {
        stagePrograss.tileRecode.Record(tile.tileType);
        stagePrograss.score += 1;
    }

    private void OnMoveTile()
    {
        if(MenuManager.Instance.currentMenu != MenuState.Main) return;

        count += 1;

        if(count > 1 )
        {
            count = 0;
            StartMiniGame();
        }
    }

    void Update()
    {
        if (isFocusMiniGame && MenuManager.Instance.currentMenu != MenuState.MiniGame)
        {
            StopMiniGame();
        }
    }

    private void StartMiniGame()
    {
        isFocusMiniGame = true;

        StartCoroutine(StateChangeAnimation(needUp, needUp.transform.position, needUp.transform.position + new Vector3(0,6), Utils.ETC_ANIMATION_TIME, EaseType.InOutCirc));

        MenuManager.Instance.ChangeMenu((int)MenuState.MiniGame);

        // if()
        
    }

    private void StopMiniGame()
    {
        isFocusMiniGame = false;

        StartCoroutine(StateChangeAnimation(needUp, needUp.transform.position, needUp.transform.position - new Vector3(0,6), Utils.ETC_ANIMATION_TIME * 2, EaseType.OutCirc));
    }


    private IEnumerator StateChangeAnimation(GameObject targetObj, Vector2 start, Vector2 target, float time, EaseType easeType){
        EventManager.Instance.ForcePause(out byte caller);

        StartCoroutine(
            Movement(
                targetObj,
                start,
                target,
                time,
                easeType
            )
        );

        //완료될때까지 대기
        yield return new WaitForSeconds(time * 2);

        EventManager.Instance.ForceRelease(caller);

    }

    private IEnumerator Movement(GameObject targetObj, Vector2 start, Vector2 target, float time, EaseType easeType)
    {
        float currentTime = 0;
        
        while(currentTime <= time){
            currentTime += Time.deltaTime;
            targetObj.transform.position = Vector2.Lerp(start, target, EaseMoveMent.Ease(easeType, currentTime / time));
            
            yield return null;
        }

        //보정
        targetObj.transform.position= target;
    }

    ~MenuMiniGame()
    {
        try
        {
            EventManager.Instance.OnMovedTile.RemoveListener(OnMoveTile);
            EventManager.Instance.OnDisabledTile.RemoveListener(OnDisabledTile);
        }
        catch (System.Exception)
        {
            // 무시
        }
    }


}
