using System.Collections;
using UnityEngine;

// 시간 경과 후 소멸하는 액션
public class TimerExtinction : DestroyerAction, IExtinctionAction
{
    // 소멸되기 까지의 시간
    [SerializeField]
    private float time;
    protected override void OnInvoke()
    {
        StartCoroutine(ExtinctionTimer());
    }

    //소멸 타이머 코루틴
    IEnumerator ExtinctionTimer()
    {
        // 지정된 시간 대기
        yield return new WaitForSeconds(time);

        tileDestroyer.Disable();
    }


}
