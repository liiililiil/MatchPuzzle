using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    Text text;
    private int score = 0;
    private void Start() {
        text = GetComponent<Text>();
        
    }
    private void FixedUpdate() {
        UpdateScore(StageManager.Instance.inStageManager.stagePrograss);
    }

    private void UpdateScore(StagePrograss stagePrograss)
    {
        if(score <= stagePrograss.score)
        {
            score += Mathf.Min(5, stagePrograss.score - score);
            text.text = score.ToString();
        }
    }

}
