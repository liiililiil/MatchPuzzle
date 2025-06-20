using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTileCalculate : TileAction, ICalculateAction
{
    private bool _isCalculated = false;
    public bool isCalculated
    {
        get => _isCalculated;
        set => _isCalculated = value;
    }


    public void CalReset()
    {
        isCalculated = false;
        tile.isCenter = false;
        tile.length = Vector2Int.zero;
    }

    protected override void OnInvoke()
    {
        if (isCalculated) return;

        tile.isCenter = true;

        Stack<Tile> totalStack = new Stack<Tile>();

        NearbyCheck(ref tile.length, ref totalStack, Vector2.zero);

        int count = totalStack.Count;
        

        //보정

        while (totalStack.Count > 0)
        {
            Tile tile = totalStack.Pop();

            tile.CalReset();


            //터지는 조건이 되면 폭발
            if (count >= 3)
            {
                tile.Blast();
            }
        }
    }

    public void NearbyCheck(ref Vector2Int length, ref Stack<Tile> totalStack, Vector2 exceptionDirection)
    {
        totalStack.Push(GetComponent<Tile>());
        isCalculated = true;
        //제외된 방향을 기반으로 체인을 증가
        if (exceptionDirection == Vector2.zero)
        {
            foreach (Vector2 dir in Utils.directions)
            {
                // 상대 방향을 절대 방향으로 변환
                Vector2 worldDir = dir.x * (Vector2)transform.right + dir.y * (Vector2)transform.up;

                ICalculateAction tile = GetTileFromWorld<ICalculateAction>(worldDir);
                if (tile != null && !tile.isCalculated)
                {
                    tile.NearbyCheck(ref length, ref totalStack, -worldDir);
                }
            }
        }
        else if (exceptionDirection == new Vector2(0, exceptionDirection.y))
        {
            length.y++;

            ICalculateAction tile = GetTileFromWorld<ICalculateAction>(-exceptionDirection);
            if (tile != null && !tile.isCalculated)
            {
                tile.NearbyCheck(ref length, ref totalStack, -exceptionDirection);
            }
        }
        else if (exceptionDirection == new Vector2(exceptionDirection.x, 0))
        {
            length.x++;

            ICalculateAction tile = GetTileFromWorld<ICalculateAction>(-exceptionDirection);
            if (tile != null && !tile.isCalculated)
            {
                tile.NearbyCheck(ref length, ref totalStack, exceptionDirection);
            }
        }
    }
}
