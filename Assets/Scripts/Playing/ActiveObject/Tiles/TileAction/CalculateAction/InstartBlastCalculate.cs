using System.Collections.Generic;
using UnityEngine;

// 즉시 폭발하는 액션
public class InstartBlastCalculate : TileAction, ICalculateAction
{
    public Vector2Int isCalculated { get; set; } = Vector2Int.zero;

    // 즉시 폭발
    protected override void OnInvoke()
    {
        if (tile.switchedTileType != TileType.Empty) tile.Blast();
    }
    
    // 연산 초기화
    public void CalReset()
    {
        isCalculated = Vector2Int.zero;
        tile.isCenter = false;
        tile.length = Vector2Int.zero;
    }

    // 주변 타일 검사 (사용 안함)
    public void NearbyCheck(ref Vector2Int length, ref Stack<Tile> totalStack, Vector2Int exceptionDirection)
    {
    }

    // 외부 접근용 타입 비교
    public bool IsEqualType(TileType type)
    {
        return tile.tileType == type;
    }
}
