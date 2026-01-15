using UnityEngine;
using UnityEngine.UI;

public class InfoDebug : MonoBehaviour
{
    [SerializeField]
    private Text leftMove;

    [SerializeField]
    private Text score;


    void Update()
    {
        leftMove.text = StageManager.Instance.inStageManager.stagePrograss.leftMovement.ToString();
        score.text = StageManager.Instance.inStageManager.stagePrograss.score.ToString();
    }

}
