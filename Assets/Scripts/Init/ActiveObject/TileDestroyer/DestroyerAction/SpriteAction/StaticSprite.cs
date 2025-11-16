using System.Collections;
using UnityEngine;

// ㅈ정적 스프라이트 액션
public class StaticSprite : SpriteAction, ISpriteAction
{
    protected override void OnInvoke()
    {
        if (sheet.GetLength(0) != 1)
        {
            Debug.LogWarning("정적 스프라이트에 두개 이상의 스프라이트가 감지되었습니다!");
        }

        StartCoroutine(OnStaticSprite());
    }

    // 정적 스프라이트 코루틴
    // 첫번째 시트를 계속 유지하다가 타일 파괴자가 비활성화 되면 스프라이트를 제거
    private IEnumerator OnStaticSprite()
    {
        spriteRenderer.sprite = sheet[0];

        while (tileDestroyer.isActive)
        {
            yield return null;
        }

        spriteRenderer.sprite = null;
    }
}
