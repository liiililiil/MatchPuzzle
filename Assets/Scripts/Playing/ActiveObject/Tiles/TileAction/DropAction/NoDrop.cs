using UnityEngine;

public class NoDrop : TileAction, IDropAction
{
    public bool isCanDrop { get { return false; } }
    protected override void OnInvoke()
    {
        // Debug.Log($"{gameObject.name}에게 드랍이 요청되었지만 드랍 타일이 아니므로 무시되었습니다.");
    }
}
