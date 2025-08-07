using UnityEngine;

public class NoFocus : TileFocusAction, ITileFocusAction
{
    public bool isCanFocus { get => false; }

    protected override void OnInvoke()
    {
        Debug.LogWarning($"{gameObject.name}에 있는 {GetType()}가 Focus를 시도했지만, 포커스가 안되는 타일이라 무시되었습니다.",this);
    }
}
