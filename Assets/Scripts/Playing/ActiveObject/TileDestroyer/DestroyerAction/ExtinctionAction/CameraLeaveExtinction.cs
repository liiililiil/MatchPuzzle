using System.Collections;
using UnityEngine;

// 카메라 시야에서 벗어나면 타일을 파괴하는 액션
public class CameraLeaveExtinction : DestroyerAction, IExtinctionAction
{
    protected override void OnInvoke()
    {
        StartCoroutine(LeaveCheck());
    }

    // 카메라 시야에서 벗어났는지 체크하는 코루틴
    IEnumerator LeaveCheck()
    {
        Vector3 viewPos;

        while (tileDestroyer.isActive)
        {
            yield return null;

            viewPos = Camera.main.WorldToViewportPoint(gameObject.transform.position);

            if (viewPos.z < 0 || viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
                tileDestroyer.Disable();
        }
    }
}
