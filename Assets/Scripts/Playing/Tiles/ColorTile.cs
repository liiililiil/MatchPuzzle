using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class ColorTile : DropTile, ITile
{
    public override sealed void Calculate(){
        Stack<ITile> totalStack = new Stack<ITile>();

        NearbyCheck(ref totalStack, Vector2.zero);

        ushort xMax = 0;
        ushort yMax = 0;
        ushort totalMax = 0;

        Debug.Log("totalStack Count: " + totalStack.Count);

        //최대값을 구합니다.
        foreach (ITile tile in totalStack) {
            xMax = (ushort)Mathf.Max(tile.xChain.self, xMax);
            yMax = (ushort)Mathf.Max(tile.yChain.self, yMax);
            totalMax = (ushort)Mathf.Max(tile.totalChain.self, totalMax);

            // Debug.Log($"{tile.xChain.self},{tile}");
        }

        // Debug.Log($"{xMax},{yMax},{totalMax}");

        foreach (ITile tile in totalStack) {
            tile.xChain.total = xMax;
            tile.yChain.total = yMax;
            tile.totalChain.total = totalMax;
            

            tile.isCalculated = false;

            //터지는 조건이 되면 폭발
            if(tile.totalChain.total >= 2){
                tile.Blasted();
            }
        }

    }

    public override sealed void NearbyCheck(ref Stack<ITile> totalStack, Vector2 exceptionDirection){

        totalStack.Push(this);
        isCalculated = true;

        //초기화
        ChainReset();

        //X축 방향으로 검사
        foreach (var direction in Utils.xDirections){

            ITile tile = Raycast(direction, 1, false);
            if (tile != null && !tile.isCalculated){
                this.xChain.self = (ushort)Mathf.Max(tile.xChain.self + 1, this.xChain.self);
                this.totalChain.self = (ushort)Mathf.Max(tile.totalChain.self + 1, this.totalChain.self);

                //재귀적으로
                if (direction != exceptionDirection) tile.NearbyCheck(ref totalStack, -direction);
            }
        }

        //Y축 방향으로 검사
        foreach (var direction in Utils.yDirections){
            if (direction == exceptionDirection) continue;

            ITile tile = Raycast(direction, 1, false);
            if (tile != null && !totalStack.Contains(tile)){
                this.yChain.self = (ushort)Mathf.Max(tile.yChain.self + 1, this.yChain.self);
                this.totalChain.self = (ushort)Mathf.Max(tile.totalChain.self + 1, this.totalChain.self);

                //재귀적으로
                if (direction != exceptionDirection) tile.NearbyCheck(ref totalStack, -direction);
            }
        }
    }

    public sealed override void Organize(){} //색 타일은 정리당하지 않음
    public sealed override void Blasted()
    {
        //4 방향으로 Ray 쏘기
        foreach (var (xDir, yDir) in Utils.xDirections.Zip(Utils.yDirections, (x, y) => (x, y)))
        {
            Vector2 direction = new Vector2(xDir.x, yDir.y);

            // Raycast를 이용해 타일을 찾고 정리 수행
            ITile tile = Raycast(direction, 1, true);
            tile?.Organize();
        }


        if (true)
        {

        }
        else
        {

        }

        // Debug.Log("터지는 조건 충족");
        // pooling();
    }
}
