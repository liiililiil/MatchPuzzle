using UnityEngine;

public class BiastExploded : TileAction, IExplodedAction
{
    protected override void OnInvoke()
    {
        tile.Blast();
    }
}
