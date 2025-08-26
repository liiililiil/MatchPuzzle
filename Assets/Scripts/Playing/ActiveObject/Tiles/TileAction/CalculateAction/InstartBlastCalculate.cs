using System.Collections.Generic;
using UnityEngine;

public class InstartBlastCalculate : TileAction, ICalculateAction
{
    public Vector2Int isCalculated { get; set; } = Vector2Int.zero;

    protected override void OnInvoke()
    {
        if(tile.switchedTileType != TileType.Empty) tile.Blast();
    }

    public void CalReset()
    {
        isCalculated = Vector2Int.zero;
        tile.isCenter = false;
        tile.length = Vector2Int.zero;
    }

    public void NearbyCheck(ref Vector2Int length, ref Stack<Tile> totalStack, Vector2Int exceptionDirection)
    {
    }
    public bool IsEqualType(TileType type)
    {
        return tile.tileType == type;
    }
}
