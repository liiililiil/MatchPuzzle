using System.Collections.Generic;
using UnityEngine;

public interface ICalculateAction : ITileAction
{
    public void NearbyCheck(ref Vector2Int length, ref Stack<Tile> totalStack, Vector2Int exceptionDirection);

    public void CalReset();
    public bool IsEqualType(TileType type);

    public Vector2Int isCalculated { get; set; }
}
