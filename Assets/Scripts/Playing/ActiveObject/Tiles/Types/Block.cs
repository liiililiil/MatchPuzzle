using System.Collections.Generic;
using UnityEngine;

public class Block : NonDropTile, ITile
{
    public override TileType tileType { get => TileType.Block; }
    public override void Calculate()
    {
    }
    public override void NearbyCheck(ref Stack<ITile> totalStack, Vector2 exceptionDirection, ushort chain)
    {
        return;
    }

    public override void Blasted(bool isCenter = false)
    {
        // 블록은 폭발하지 않음
        return;
    }

    public override void Organize()
    {
        // 블록은 폭발하지 않음
        return;
    }


}
