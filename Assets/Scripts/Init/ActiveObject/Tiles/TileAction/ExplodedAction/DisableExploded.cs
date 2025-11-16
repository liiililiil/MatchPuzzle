using UnityEngine;

public class DisableExploded : TileExplodeAction, IExplodedAction
{
    protected override void OnInvoke()
    {
        tile.Disable();
    }
}
