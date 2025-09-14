// 파괴 효과 없음 액션
public class NotEffect : DestroyerAction, IEffectSpawnAction
{
    protected override void OnInvoke()
    {
        // 아무 동작도 하지 않음
    }
}
