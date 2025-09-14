using System.Collections;
using UnityEngine;

// 시트로 된 스프라이트를 재생하는 액션
public class SheetedSpriteChange : SpriteChangeAction, ISpriteChangeAction
{
    protected override void OnInvoke()
    {
        // 시트 재생 코루틴 시작
        StartCoroutine(AnimationPlay());
    }

    // 시트 재생 코루틴
    protected IEnumerator AnimationPlay()
    {
        // 시트의 스프라이트 개수
        int count = sheet.Length;

        // 이펙트가 비활성화 될 때까지 반복
        while (effect.isActive)
        {

            // 시트의 스프라이트를 순서대로 재생
            for (int i = 0; i < count; i++)
            {
                // 이펙트가 비활성화 되었으면 중지
                if (!effect.isActive) break;

                // 스프라이트 변경
                spriteRenderer.sprite = sheet[i];

                // 다음 프레임까지 대기
                float time = 0f;
                while (time <= 1f)
                {
                    time += Time.deltaTime * Utils.EFFECT_SPEED;
                    yield return null;
                }

            }
        }

    }
}
