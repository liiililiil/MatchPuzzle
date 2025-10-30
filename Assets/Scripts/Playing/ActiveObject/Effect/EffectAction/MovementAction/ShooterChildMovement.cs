using System.Collections;
using UnityEngine;

public class ShooterChildMovement : EffectAction, IEffectMovementAction
{
    public Vector2 targetPosition;
    protected override void OnInvoke()
    {
        // Debug.Log("슈터 자식 이동 액션 시작", this);

        StartCoroutine(Animation());
    }

    IEnumerator Animation()
    {
        float time = 0f;
        Vector2 startPosition = transform.position;
        Vector2 firstTargetPosition = startPosition + new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        Vector2 targetPos = Vector2.zero;

        float angle = Mathf.Atan2(firstTargetPosition.y - startPosition.y, firstTargetPosition.x - startPosition.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        while (time < 1f)
        {
            time += Time.deltaTime * Utils.SHOOTER_CHILD_MOVE_SPEED;

            //움직임
            if (time < 0.5f)
            {
                targetPos = Vector2.Lerp(startPosition, firstTargetPosition, EaseMoveMent.Ease(EaseType.OutExpo, time * 2f));
            }
            else if (time >= 0.5f)
            {
                targetPos = Vector2.Lerp(firstTargetPosition, targetPosition, EaseMoveMent.Ease(EaseType.InExpo, (time - 0.5f) * 2f));
            }
            
            //회전
            if(time < 0.5f)
            {
            float targetAngle = Mathf.Atan2(targetPosition.y - firstTargetPosition.y, targetPosition.x - firstTargetPosition.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpUnclamped(angle , targetAngle, EaseMoveMent.Ease(EaseType.InOutCirc, time *2f)));
            }
            
            transform.position = targetPos;
            yield return null;
            
        }

        transform.position = targetPosition;
    }
}
