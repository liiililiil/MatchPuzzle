using UnityEngine;

public class TileFocusAction : TileAction
{
    public virtual void OnFocus(Vector2Int at)
    {
        Debug.LogWarning("포커싱 설정되지 않음!", this);
    }

    public virtual void UnFocus()
    {
        Debug.LogWarning("포커싱 설정되지 않음!", this);
    }
    
    public virtual void Move(Vector2Int moveTo)
    {
        Debug.LogWarning("이동 설정되지 않음!", this);
    }
}
