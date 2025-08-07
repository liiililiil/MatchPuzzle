using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorTileCalculate : TileAction, ICalculateAction
{
    private Vector2Int _isCalculated = Vector2Int.zero;
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

    protected override void OnInvoke()
    {
        if (isCalculated != Vector2Int.zero) return;

        // Debug.Log("계산 시작", gameObject);

        tile.isCenter = true;

        Stack<Tile> totalStack = new Stack<Tile>();


        tile.length = Vector2Int.one;

        NearbyCheck(ref tile.length, ref totalStack, Vector2Int.zero);

        // Debug.Log(tile.length);

        bool needBlasted = tile.length.x >= 3 || tile.length.y >= 3;

        // Debug.Log($"계산 완료 : {gameObject.name} - {tile.length} ({tile.tileType})", gameObject);


        //보정

        while (totalStack.Count > 0)
        {
            Tile tile = totalStack.Pop();

            tile.CalReset();


            //터지는 조건이 되면 폭발
            if (needBlasted)
            {
                tile.Blast();
            }
        }
    }

    public void NearbyCheck(ref Vector2Int length, ref Stack<Tile> totalStack, Vector2Int exceptionDirection)
    {

        totalStack.Push(GetComponent<Tile>());
        // isCalculated = true;
        if (exceptionDirection == Vector2Int.zero)
        {
            
        }
        else if (exceptionDirection.x == 0)
        {
            if (_isCalculated.y != 0)
            {
                // Debug.Log("이미 계산된 방향입니다.", this);
                return;
            }
            length.y++;
            _isCalculated.y++;
        }
        else if (exceptionDirection.y == 0)
        {
            if (_isCalculated.x != 0)
            {
                // Debug.Log("이미 계산된 방향입니다.", this);
                return;
            }

            length.x++;
            _isCalculated.x++;
        }

        // Debug.Log($"주변 연산 시작 | 제외 : {exceptionDirection} | 현황 : {length} | 계산 여부 : {_isCalculated}",this);

        //들어온 방향 축 먼저 연산
        Vector2Int reverseDirection = -exceptionDirection;

        Vector2Int[] sortedDirs = Utils.directions
            .OrderByDescending(dir => dir == reverseDirection ? 1 : 0)
            .ToArray();

        foreach (Vector2Int dir in sortedDirs)
        {
            if (dir == exceptionDirection) continue;

            Vector2 worldDir = dir.x * (Vector2)transform.right + dir.y * (Vector2)transform.up;

            ICalculateAction tile = GetTileFromWorld<ICalculateAction>(worldDir);
            if (tile != null && tile.IsEqualType(this.tile.tileType))
            {
                if (exceptionDirection == -dir || exceptionDirection == Vector2Int.zero)
                {
                    tile.NearbyCheck(ref length, ref totalStack, -dir);
                }
                else
                {
                    Vector2Int temp = Vector2Int.one;

                    // Debug.Log("새로운 각도 연산 시작", this);
                    tile.NearbyCheck(ref temp, ref totalStack, -dir);

                    // Debug.Log($"새로운 각도 연산 완료 : {temp} | 기존 : {length}", this);
                    length = Vector2Int.Max(length, temp);
                }
            }
        }
    }
    
    public bool IsEqualType(TileType type)
    {
        return tile.tileType == type;
    }
    

}
