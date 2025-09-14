using System.Collections;
using UnityEngine;

// 애니메이션 완료 후 타일을 파괴하는 액션
public class AnimationComplateExtinction : DestroyerAction, IExtinctionAction
{
    protected override void OnInvoke()
    {
        // 애니메이션 시트 길이를 가져와서 그 길이만큼 대기 후 타일 파괴
        StartCoroutine(startExtinctionTimer(GetComponent<ISpriteAction>().GetLenght()));
    }

    // 애니메이션 재생이 모두 끝났는지 예측하는 코루틴
    IEnumerator startExtinctionTimer(int count)
    {

        while (count > 0)
        {
            count--;

            float time = 0f;
            while (time <= 1f)
            {
                time += Time.deltaTime * Utils.EFFECT_SPEED;
                yield return null;
            }
        }

        tileDestroyer.Disable();
    }
}
