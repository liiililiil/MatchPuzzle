using UnityEngine;

public class FailToMenuButton : MonoBehaviour
{
    public void Onclick()
    {
        StageManager.Instance.StageClose();
    }
}
