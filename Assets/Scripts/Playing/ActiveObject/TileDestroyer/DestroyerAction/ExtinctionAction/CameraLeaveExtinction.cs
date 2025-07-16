using System.Collections;
using UnityEngine;

public class CameraLeaveExtinction : DestroyerAction, IExtinctionAction
{
    protected override void OnInvoke()
    {
        StartCoroutine(LeaveCheck());
    }

    IEnumerator LeaveCheck()
    {
        Vector3 viewPos;
        while (true)
        {
            viewPos = Camera.main.WorldToViewportPoint(gameObject.transform.position);

            if (viewPos.z < 0 || viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
            {
                tileDestroyer.Disable();
                break;
            }
            
            yield return null;
                
        }
    }
}
