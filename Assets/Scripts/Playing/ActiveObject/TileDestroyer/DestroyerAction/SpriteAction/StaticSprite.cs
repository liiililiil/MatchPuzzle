using System.Collections;
using UnityEngine;

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

    private IEnumerator OnStaticSprite()
    {
        spriteRenderer.sprite = sheet[0];

        //변경완료!

        while (tileDestroyer.isActive)
        {
            yield return null;
        }

        spriteRenderer.sprite = null;
    }
}
