using System.Collections;
using UnityEngine;

public class StraightDestroyer : DestroyerAction, IMovementAction
{
    new Rigidbody2D rigidbody2D;
    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        if (rigidbody2D == null)
        {
            Debug.LogError($"{gameObject.name}에 Rigidbody2D가 없습니다!");
        }
    }
    protected override void OnInvoke()
    {
        StartCoroutine(Forward());
    }

    IEnumerator Forward()
    {
        while (tileDestroyer.isActive)
        {
            rigidbody2D.MovePosition(transform.position + transform.up * Utils.TILE_GAP);
            yield return null;
        }

        rigidbody2D.linearVelocity = Vector2.zero;
    }
    
}
