using System.Collections.Generic;
using UnityEngine;

// 계산하지 않는 액션
public class NoCalculate : TileCalCulateAction, ICalculateAction
{

    protected override void OnInvoke()
    {
    }
    public void NearbyCheck(ref Vector2Int length, ref Stack<Tile> totalStack, Vector2Int exceptionDirection)
    {
    }

}
