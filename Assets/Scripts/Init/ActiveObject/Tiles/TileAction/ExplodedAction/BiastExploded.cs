using UnityEngine;

public class BiastExploded : TileExplodeAction, IExplodedAction
{
    protected override void OnInvoke()
    {
        tile.Blast();
    }
}
