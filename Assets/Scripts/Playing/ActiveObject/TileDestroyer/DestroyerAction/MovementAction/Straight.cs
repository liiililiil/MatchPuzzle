using System.Collections;
using UnityEngine;

public class StraightDestroyer : DestroyerAction, IMovementAction
{
    private Transform sprite;
    private Coroutine coroutine;
    protected override void OnInvoke()
    {
        if (sprite == null) sprite = transform.GetChild(0).GetComponent<Transform>();
        coroutine = StartCoroutine(moveMent2D(transform.up));
    }
        IEnumerator moveMent2D(Vector2 direction)
    {
        // 현재 위치를 그리드에 정렬
        Vector2 startPos = new Vector2(
            Mathf.Round(transform.position.x / Utils.TILE_GAP) * Utils.TILE_GAP,
            Mathf.Round(transform.position.y / Utils.TILE_GAP) * Utils.TILE_GAP
        );

        //이동 위치
        Vector2 targetPos = startPos + direction * Utils.TILE_GAP;

        //히트박스는 미리 이동해놓기 
        transform.position = targetPos;

        float time = 0f;
        while (time <= 1f)
        {
            sprite.transform.position = Vector2.Lerp(startPos, targetPos, time);
            time += Time.deltaTime * Utils.MOVEMENT_SPEED;
            yield return null;
        }

        // 위치 정밀 보정
        sprite.position = targetPos;

        //하강 완료
        coroutineEnd();
    }

    protected void coroutineEnd()
    {
        StopCoroutine(coroutine);
        coroutine = null;

        EventManager.Instance.InvokeDestroyerMove += Invoke;
        
    }
    
}
