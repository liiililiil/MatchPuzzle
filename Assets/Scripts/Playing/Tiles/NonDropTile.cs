using UnityEngine;

public abstract class NonDropTile : Tile, ITile
{
    public override sealed void Drop()
    {
        return;
    }
}
