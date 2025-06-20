using UnityEngine;

public class NoOranize : TileAction, IOrganizeAction
{
    protected override void OnInvoke()
    {
        Debug.Log($"{gameObject.name}에게 정리가 요청되었지만 정리 타일이 아니므로 무시되었습니다.");
    }
}
