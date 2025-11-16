using System.Collections;
using UnityEngine;


// 스프라이트가 모두 재생되면 비활성화
public class SpritePlayCompleteExtinction : EffectAction, IEffectExtinctionAction
{
    // 재생될 스프라이트 개수
    private byte count;
    protected override void OnInvoke()
    {
        // ISpriteChangeAction에서 스프라이트 개수 가져오기
        try{
            count = GetComponent<ISpriteChangeAction>().spriteCount;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"에러 발생! {ex}");
        }

        // 스트라이트 재생을 간접적으로 기록하기 위해 코루틴 시작
        StartCoroutine(startExtinctionTimer());
    }

    // 스프라이트 재생이 모두 끝났는지 예측하는 코루틴
    IEnumerator startExtinctionTimer()
    {
        // 스프라이트 재생이 모두 끝날 때까지 대기
        while (count > 0)
        {
            count--;
            float time = 0f;

            while (time <= 1f)
            {
                time += Time.deltaTime * GameSpeedManager.EFFECT_DURATION;
                yield return null;
            }
        }

        // 재생이 모두 끝났으면 이펙트 비활성화
        effect.Disable();
    }
}
