using System.Collections;
using UnityEngine;

public class ShooterChildExtinction : EffectAction, IEffectExtinctionAction
{
    protected override void OnInvoke()
    {
        StartCoroutine(startExtinctionTimer());
    }
    
    IEnumerator startExtinctionTimer()
    {
        float time = 0f;
        while (time <= 1)
        {
            time += Time.deltaTime * GameSpeedManager.SHOOTER_CHILD_MOVE_SPEED;
            yield return null;
            // Debug.Log(time);
        }

        // 1 프레임 대기  
        yield return null;
        effect.Disable();
    }
}
