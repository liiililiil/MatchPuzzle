using UnityEngine;

public class TileCalCulateAction : TileAction
{
    protected Vector2Int _isCalculated;
    public Vector2Int isCalculated { get => _isCalculated; set => _isCalculated = value; }
    public void CalReset()
    {
        isCalculated = Vector2Int.zero;
        tile.length = Vector2Int.zero;
        tile.isCenter = false;
    }
    public bool IsEqualType(TileType type)
    {
        return tile.tileType == type;
    }
}
