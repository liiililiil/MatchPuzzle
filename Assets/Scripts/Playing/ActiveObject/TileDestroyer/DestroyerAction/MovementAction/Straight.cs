using System.Collections;
using UnityEngine;

// 직선으로 이동하는 액션
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

    // 직선 이동 코루틴
    IEnumerator Forward()
    {
            
        // 타일 파괴자가 활성화 되어있는 동안 계속 이동
        while (tileDestroyer.isActive)
        {
            rigidbody2D.linearVelocity = transform.up * GameSpeedManager.DESTROYER_FORWARD_SPEED;   
            yield return null;
        }

        // 이동이 멈추면 속도 0으로 고정
        rigidbody2D.linearVelocity = Vector2.zero;
    }

}
