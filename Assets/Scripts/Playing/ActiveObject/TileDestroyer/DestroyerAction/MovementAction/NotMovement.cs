using UnityEngine;

// 이동하지 않는 액션
public class NotMovement : DestroyerAction, IMovementAction
{
    private new Rigidbody2D rigidbody;
    protected override void OnInvoke()
    {
        if (rigidbody == null) rigidbody = GetComponent<Rigidbody2D>();

        //위치 교정
        rigidbody.MovePosition(transform.position);
    }
}
