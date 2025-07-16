using System.Collections;
using UnityEngine;

public class SheetedSpriteChange : SpriteChangeAction, ISpriteChangeAction
{
    private void Start() {
        
    }
    protected override void OnInvoke()
    {
        StartCoroutine(AnimationPlay());
    }

    protected IEnumerator AnimationPlay()
    {
        int count = sheet.Length;

        while (effect.isActive)
        {


            for (int i = 0; i < count; i++)
            {
                if (!effect.isActive) break;

                spriteRenderer.sprite = sheet[i];

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
