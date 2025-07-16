using System.Collections;
using UnityEngine;

public class SpritePlayCompleteExtinction : EffectAction, IEffectExtinctionAction
{
    private int count;
    protected override void OnInvoke()
    {
        try
        {
            count = GetComponent<ISpriteChangeAction>().spriteCount;

        }
        catch (System.Exception ex)
        {
            Debug.LogError($"에러 발생! {ex}");
        }

        StartCoroutine(startExtinctionTimer());

    }

    IEnumerator startExtinctionTimer()
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

        effect.Disable();
    }
}
