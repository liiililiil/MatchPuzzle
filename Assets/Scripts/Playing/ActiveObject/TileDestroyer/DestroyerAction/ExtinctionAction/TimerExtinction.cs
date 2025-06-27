using System.Collections;
using System.Threading;
using UnityEngine;

public class TimerExtinction : DestroyerAction, IExtinctionAction
{
    [SerializeField]
    private float time;
    protected override void OnInvoke()
    {
        StartCoroutine(ExtinctionTimer());
    }

    IEnumerator ExtinctionTimer()
    {
        yield return new WaitForSeconds(time);

        tileDestroyer.Disable();
    }

    
}
