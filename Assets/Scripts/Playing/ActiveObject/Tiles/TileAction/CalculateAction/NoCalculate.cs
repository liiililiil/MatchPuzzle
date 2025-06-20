using System.Collections.Generic;
using UnityEngine;

public class NoCalculate : TileAction, ICalculateAction
{
    public bool isCalculated { get; set; } = false;

    protected override void OnInvoke()
    {
    }

    public void CalReset()
    {
        isCalculated = false;
        tile.isCenter = false;
        tile.length = Vector2Int.zero;
    }

    public void NearbyCheck(ref Vector2Int length, ref Stack<Tile> totalStack, Vector2 exceptionDirection)
    {
    }
    
}
