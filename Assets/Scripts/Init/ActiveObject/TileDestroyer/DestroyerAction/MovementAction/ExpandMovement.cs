using System.Collections;
using UnityEngine;

public class ExpandMovement : DestroyerAction, IMovementAction
{
    [SerializeField]
    private Vector2 initialScale;
    [SerializeField]
    private Vector2 targetScale;
    protected override void OnInvoke()
    {
        StartCoroutine(ScaleIncrease());
    }

        IEnumerator ScaleIncrease()
    {
        float time = 0f;
        while (time < 1f)
        {
            time += Time.deltaTime * GameSpeedManager.SHOOTER_SCALE_SPEED;
            tileDestroyer.gameObject.transform.localScale = Vector2.Lerp(initialScale, targetScale, time);
            yield return null;
        }
        tileDestroyer.gameObject.transform.localScale = targetScale;
    }

}
