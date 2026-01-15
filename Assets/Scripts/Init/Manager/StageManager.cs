using UnityEngine;

public class StageManager : Managers<StageManager>
{
    
    public IInStageManager inStageManager { get; private set; }

    [SerializeField]
    private GameObject failGameObject;

    private byte caller;

    private SimpleEvent OnFail = new SimpleEvent();
    private SimpleEvent OnSuccess = new SimpleEvent();
    private SimpleEvent OnStageStart = new SimpleEvent();
    private SimpleEvent OnStageClose = new SimpleEvent();
    
    //버튼에서 쓰는 함수들
    public void StageStart()
    {
        EventManager.Instance.ForceRelease(caller);

        OnStageStart.Invoke();
    }

    public void StageClose()
    {
        failGameObject.SetActive(false);

        OnStageClose.Invoke();
        SceneLoadManager.Instance.SceneLoad("Menu", () => MenuManager.Instance.ChangeMenu(3));
    }


    //InStageManager에서 쓰는 함수
    public void GameStart(bool isSilence = false)
    {
        EventManager.Instance.ForcePause(out caller);

        OnStageStart.Invoke();
    }
  
    public void Fail(string cause, bool isSilence = false)
    {
        EventManager.Instance.ForcePause(out caller);
        failGameObject.GetComponent<GameOverDisplay>().DisplayedGameOver(cause,inStageManager.stagePrograss.tileRecode);

        OnFail.Invoke();
    }

    public void Success()
    {
        EventManager.Instance.ForcePause(out caller);

        OnSuccess.Invoke();
    }



    public void bind(IInStageManager inStageManager_)
    {
        inStageManager = inStageManager_;
    }
}
