using UnityEngine;

public abstract class NonDropTile : Tile, ITile
{
    public override bool isCanDrop { get => false; }
    public sealed override void Drop()
    {
    }
}
