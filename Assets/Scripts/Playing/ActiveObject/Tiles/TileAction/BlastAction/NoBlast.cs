using UnityEngine;

public class NoBlast : BlastAction, IBlastAction
{
    protected override void OnInvoke()
    {
        Debug.Log($"{gameObject.name}에게 폭발이 요청되었지만 폭발 타일이 아니므로 무시되었습니다.");
    }

    public void CenterBlast()
    {
        Debug.Log($"{gameObject.name}에게 중심 폭발이 요청되었지만 폭발 타일이 아니므로 무시되었습니다.");
    }
}
