using System.Collections.Generic;
using UnityEngine;

// 즉시 폭발하는 액션
public class InstartBlastCalculate : TileCalCulateAction, ICalculateAction
{

    // 즉시 폭발
    protected override void OnInvoke()
    {
        if (tile.switchedTileType != TileType.Empty) tile.Blast();
    }
    

    // 주변 타일 검사 (사용 안함)
    public void NearbyCheck(ref Vector2Int length, ref Stack<Tile> totalStack, Vector2Int exceptionDirection)
    {
    }

}
