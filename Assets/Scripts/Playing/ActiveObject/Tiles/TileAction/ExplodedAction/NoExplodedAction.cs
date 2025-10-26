using UnityEngine;

public class NoExplodedAction : TileExplodeAction, IExplodedAction
{
    protected override void OnInvoke()
    {
        // Debug.Log("폭발당하지 않는 타일이라 무시되었습니다.");
    }
}
