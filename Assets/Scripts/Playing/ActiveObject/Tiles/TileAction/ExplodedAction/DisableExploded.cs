using UnityEngine;

public class DisableExploded : TileAction, IExplodedAction
{
    protected override void OnInvoke()
    {
        tile.Disable();
    }
}
