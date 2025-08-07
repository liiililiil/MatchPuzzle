using System.Collections;
using UnityEngine;

public class CollisionDestroy : DestroyerAction, IDestroyAction
{
    private BoxCollider2D boxCollider2D;
    protected override void OnInvoke()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();

        if (boxCollider2D == null) Debug.LogError("Box Collider가 없습니다!");

        StartCoroutine(CallDestroy());
    }

    IEnumerator CallDestroy()
    {
        boxCollider2D.enabled = true;

        while (tileDestroyer.isActive)
        {
            yield return null;
        }

        boxCollider2D.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("콜리전!",this);
        collision.gameObject.GetComponent<Tile>()?.hit(tileDestroyer);
        // Debug.Log("충돌!");
    }

}
