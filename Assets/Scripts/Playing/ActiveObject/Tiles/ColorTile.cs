using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class ColorTile : DropTile, ITile
{
    public override sealed void Calculate()
    {
        //계산 당했다면 안하기
        if (isCalculated) return;

        Stack<ITile> totalStack = new Stack<ITile>();
        NearbyCheck(ref totalStack, Vector2.zero, 0);

        ushort xMax = 0;
        ushort yMax = 0;
        // ushort totalMax = 0;

        Debug.Log("totalStack Count: " + totalStack.Count);
        int count = totalStack.Count;

        //최대값을 구합니다.
        foreach (ITile tile in totalStack)
        {
            xMax = (ushort)Mathf.Max(tile.xChain.self, xMax);
            yMax = (ushort)Mathf.Max(tile.yChain.self, yMax);
            // totalMax = (ushort)Mathf.Max(tile.totalChain.self, totalMax);
        }

        while (totalStack.Count > 0)
        {
            ITile tile = totalStack.Pop();
            tile.xChain.total = xMax;
            tile.yChain.total = yMax;
            // tile.totalChain.total = totalMax;

            EventManager.Instance.OnCalReset += tile.CalReset;

            //터지는 조건이 되면 폭발
            if (count >= 3)
            {
                if (totalStack.Count <= 1) tile.Blasted(true);
                else tile.Blasted();
            }
        }

    }

    public override sealed void NearbyCheck(ref Stack<ITile> totalStack, Vector2 exceptionDirection, ushort chain)
    {
        totalStack.Push(this);
        isCalculated = true;

        //제외된 방향을 기반으로 체인을 증가
        if (exceptionDirection == Vector2.zero);
        else if (exceptionDirection == new Vector2(0, exceptionDirection.y)) yChain.self = chain;
        else if (exceptionDirection == new Vector2(exceptionDirection.x, 0)) xChain.self = chain;


        //X축 방향으로 검사
        foreach (var direction in Utils.xDirections)
        {

            ITile tile = GetTileFromWorld(direction);
            if (tile != null)
            {
                // this.xChain.self = (ushort)Mathf.Max(tile.xChain.self + 1, xChain.self);
                // this.totalChain.self = (ushort)Mathf.Max(tile.totalChain.self + 1, totalChain.self);

                //재귀적으로
                if (direction != exceptionDirection && !tile.isCalculated)
                {
                    tile.NearbyCheck(ref totalStack, -direction, (ushort)(xChain.self + 1));
                    xChain += tile.xChain;
                }
            }
        }

        //Y축 방향으로 검사
        foreach (var direction in Utils.yDirections)
        {
            if (direction == exceptionDirection) continue;

            ITile tile = GetTileFromWorld(direction);
            if (tile != null)
            {
                // this.yChain.self = (ushort)Mathf.Max(tile.yChain.self + 1, yChain.self);
                // this.totalChain.self = (ushort)Mathf.Max(tile.totalChain.self + 1, totalChain.self);

                //재귀적으로
                if (direction != exceptionDirection && !tile.isCalculated)
                {
                    tile.NearbyCheck(ref totalStack, -direction, (ushort)(yChain.self + 1));
                    yChain += tile.yChain;
                }
            }
        }
    }

    public sealed override void Organize() { } //색 타일은 정리당하지 않음
    public sealed override void Blasted(bool isCenter = false)
    {
        EventManager.Instance.OnBlastedTile.Invoke(this, transform.position);

        //4 방향으로 Ray 쏘기
        foreach (var (xDir, yDir) in Utils.xDirections.Zip(Utils.yDirections, (x, y) => (x, y)))
        {
            Vector2 direction = new Vector2(xDir.x, yDir.y);

            // GetTileFromWorld를 이용해 타일을 찾고 정리 수행
            ITile tile = GetTileFromWorld(direction, true);
            tile?.Organize();
        }

        if(isCenter) CalculateSpawnBomb();

        // Debug.Log("터지는 조건 충족");
        Disable();
    }

    private void CalculateSpawnBomb()
    {
        
    }
}
