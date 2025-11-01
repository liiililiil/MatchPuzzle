using System.Collections;
using UnityEngine;

// 애니메이션 스프라이트 액션
public class AnimatedSprite : SpriteAction, ISpriteAction
{
    protected override void OnInvoke()
    {
        StartCoroutine(AnimationPlay());
    }

    // 애니메이션 재생 코루틴

    protected IEnumerator AnimationPlay()
    {
        int count = sheet.Length;


        // 타일 파괴자가 활성화 되어있는 동안 계속 애니메이션 재생
        while (tileDestroyer.isActive)
        {

            
            for (int i = 0; i < count; i++)
            {
                if (!tileDestroyer.isActive) break;

                spriteRenderer.sprite = sheet[i];

                float time = 0f;
                while (time <= 1f)
                {
                    time += Time.deltaTime * Utils.EFFECT_DURATION;
                    yield return null;
                }

            }
        }

    }
}
