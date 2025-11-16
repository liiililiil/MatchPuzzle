using UnityEngine;

public class TileCalCulateAction : TileAction
{
    // 연산이 완료되었는지 여부
    protected Vector2Int _isCalculated = Vector2Int.zero;

    //밖 접근용
    public Vector2Int isCalculated
    {
        get => _isCalculated;
        set => _isCalculated = value;
    }
    public void CalReset()
    {
        isCalculated = Vector2Int.zero;
        tile.isCenter = false;
        tile.length = Vector2Int.one;
    }

    // 외부 접근용 타입 비교
    public bool IsEqualType(TileType type)
    {
        return tile.tileType == type;
    }

}
