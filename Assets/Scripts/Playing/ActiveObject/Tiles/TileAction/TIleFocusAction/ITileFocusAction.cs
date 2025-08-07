

using UnityEngine;

public interface ITileFocusAction : ITileAction
{
    public bool isCanFocus { get; }
    public void OnFocus(Vector2Int at);
    public void UnFocus();
    public void Move(Vector2Int moveTo);
}
