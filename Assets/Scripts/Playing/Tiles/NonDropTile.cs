using UnityEngine;

public abstract class NonDropTile : Tile, ITile
{
    public sealed override void Drop()
    {
    }
}
