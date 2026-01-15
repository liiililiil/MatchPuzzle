using UnityEngine;

public class DebugFail : MonoBehaviour
{
    public void OnClick(){
        StageManager.Instance.Fail("디버그");
    }
}
