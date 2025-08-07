using System.Collections;
using UnityEngine;

public class AnimatedSprite : SpriteAction, ISpriteAction{
    protected override void OnInvoke()
    {
        StartCoroutine(AnimationPlay());
    }

    protected IEnumerator AnimationPlay()
    {
        int count = sheet.Length;

        while (tileDestroyer.isActive)
        {


            for (int i = 0; i < count; i++)
            {
                if (!tileDestroyer.isActive) break;

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
