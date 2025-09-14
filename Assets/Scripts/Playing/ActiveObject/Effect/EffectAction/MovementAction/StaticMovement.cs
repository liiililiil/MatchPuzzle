using UnityEngine;

// 움직이지 않는 이펙트
public class StaticMovement : EffectAction, IEffectMovementAction
{

    // 아무 동작도 하지 않음
    protected override void OnInvoke()
    {
        // 움직이지 않음
    }
}
