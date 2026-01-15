using UnityEngine;

public class FailButton : MonoBehaviour
{
    void OnCilck()
    {
        StageManager.Instance.Fail("디버그");
    }
}
