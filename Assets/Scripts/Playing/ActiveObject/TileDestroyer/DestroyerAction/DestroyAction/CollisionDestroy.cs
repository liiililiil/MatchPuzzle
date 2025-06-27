using UnityEngine;

public class CollisionDestroy : DestroyerAction, IDestroyAction
{
    private BoxCollider2D boxCollider2D;
    protected override void OnInvoke()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();

        if (boxCollider2D == null) Debug.LogError("Box Collider가 없습니다!");

    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<Tile>()?.hit(tileDestroyer);
    }

}
