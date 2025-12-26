using UnityEngine;

public class StageManager : Managers<StageManager>
{
    
    public IInStageManager inStageManager { get; private set; }
    
    private void StageStart()
    {

    }

    private void StageClose()
    {

    }

    public void Fail()
    {
        Debug.Log("Stage Fail");
    }

    public void Success()
    {
        Debug.Log("Stage Success");
    }

    public void bind(IInStageManager inStageManager_)
    {
        inStageManager = inStageManager_;
    }
}
