using System.Collections.Generic;
using UnityEngine;



public abstract class InStageManager : MonoBehaviour
{
    public StagePrograss stagePrograss { get; protected set; } = new StagePrograss();

    [SerializeField]
    protected StageClearTargetPreset stageClearTarget;
    private void Start() {
        StageManager.Instance.bind(this as IInStageManager);
        OnStart();
    }

    protected virtual void OnStart()
    {
        Debug.Log("시작시 명령이 설정되지 않음");
    }

    protected void Init(int score_, int leftMovement_)
    {
        stagePrograss.score = score_;
        stagePrograss.leftMovement = leftMovement_;

        stagePrograss.tileRecode.Clear();

    }

    protected void StageCheck()
    {
        if(CheckClearTarget())
        {
            StageManager.Instance.Success();
            return;
        }
        
        if(stagePrograss.leftMovement != -1 && stagePrograss.leftMovement <= 0)
        {
            StageManager.Instance.Fail("남은 움직임 없음!");
            return;
        }
    }

    protected bool CheckClearTarget()
    {
        if(stageClearTarget == null)
            return false;
            
        int count = 0;

        foreach (var target in stageClearTarget.stageClearTargets)
        {
            foreach (var record in stagePrograss.tileRecode.GetRecord())
            {
                if (target.type == (TileType)stagePrograss.tileRecode.GetRecord().IndexOf(record))
                {
                    if (record < target.targetCount)
                    {
                        return false;
                    }

                    count++;
                    break;
                }
            }
        }

        if (count != stageClearTarget.stageClearTargets.Length)
            return false;

        return true;
    }
}
