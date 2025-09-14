using System.Collections.Generic;
using UnityEngine;

// 계산하지 않는 액션
public class NoCalculate : TileAction, ICalculateAction
{
    public Vector2Int isCalculated { get; set; } = Vector2Int.zero;

    protected override void OnInvoke()
    {
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
