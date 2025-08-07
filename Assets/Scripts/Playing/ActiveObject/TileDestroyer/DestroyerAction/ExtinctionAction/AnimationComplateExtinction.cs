using System.Collections;
using UnityEngine;

public class AnimationComplateExtinction : DestroyerAction, IExtinctionAction
{
    protected override void OnInvoke()
    {
        StartCoroutine(startExtinctionTimer(GetComponent<ISpriteAction>().GetLenght()));
    }
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
