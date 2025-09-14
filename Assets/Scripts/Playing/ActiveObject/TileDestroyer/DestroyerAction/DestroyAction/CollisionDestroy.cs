using System.Collections;
using UnityEngine;

// 충돌로 타일을 파괴하는 액션
public class CollisionDestroy : DestroyerAction, IDestroyAction
{
    private BoxCollider2D boxCollider2D;


    protected override void OnInvoke()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        if (boxCollider2D == null) Debug.LogError("Box Collider가 없습니다!");

        StartCoroutine(CallDestroy());
    }

    // 파괴자가 활성화 중인 동안만 콜라이더를 활성화
    IEnumerator CallDestroy()
    {
        boxCollider2D.enabled = true;

        while (tileDestroyer.isActive)
        {
            yield return null;
        }

        boxCollider2D.enabled = false;
    }

    // 타일과 충돌 시 타일 파괴 호출 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Tile>()?.hit(tileDestroyer);
    }

}
