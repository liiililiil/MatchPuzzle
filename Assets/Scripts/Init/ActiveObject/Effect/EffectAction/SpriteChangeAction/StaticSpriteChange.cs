using System.Collections;
using UnityEngine;

// 정적인 스프라이트 액션
public class StaticSpriteChange : SpriteChangeAction, ISpriteChangeAction
{
    protected override void OnInvoke()
    {
        if (sheet.Length == 0)
        {
            Debug.LogError($"{gameObject.name}에 있는 {GetType()}가 스프라이트 시트를 가지고 있지 않습니다!");
            return;
        }
        else if (sheet.Length > 1)
        {
            Debug.LogWarning($"{gameObject.name}에 있는 {GetType()}가 여러 스프라이트 시트를 가지고 있습니다!");
        }
        
        spriteRenderer.sprite = sheet[0];
    }
}
