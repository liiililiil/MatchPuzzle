using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

        NearbyCheck(ref tile.length, ref totalStack, Vector2Int.zero);

        Debug.Log(tile.length);

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

    public void NearbyCheck(ref Vector2Int length, ref Stack<Tile> totalStack, Vector2Int exceptionDirection)
    {
        totalStack.Push(GetComponent<Tile>());
        isCalculated = true;

        if (exceptionDirection.x == 0)
        {
            length.y++;
        }
        else if (exceptionDirection.y == 0)
        {
            length.x++;
        }

        foreach (Vector2Int dir in Utils.directions)
        {
            if (dir == exceptionDirection) continue;

            // 상대 방향을 절대 방향으로 변환
            Vector2 worldDir = dir.x * (Vector2)transform.right + dir.y * (Vector2)transform.up;

            ICalculateAction tile = GetTileFromWorld<ICalculateAction>(worldDir);
            if (tile != null && !tile.isCalculated)
            {
                // Debug.Log($"제외 : {exceptionDirection} 현재 : {dir}");

                //같은 줄일때
                if (exceptionDirection == -dir || exceptionDirection == Vector2Int.zero)
                {
                    tile.NearbyCheck(ref length, ref totalStack, -dir);
                    // Debug.Log($"같은줄 제외 : {exceptionDirection} 현재 : {dir}");
                }
                else
                {
                    Vector2Int temp = new Vector2Int();

                    tile.NearbyCheck(ref temp, ref totalStack, -dir);

                    length = Vector2Int.Max(length, temp);
                }

            }
        }
    }
    

}
