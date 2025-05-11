using UnityEngine;
using System.Collections.Generic;

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
            xMax = (ushort)Mathf.Max(tile.xChainSelf, xMax);
            yMax = (ushort)Mathf.Max(tile.yChainSelf, yMax);
            totalMax = (ushort)Mathf.Max(tile.totalChainSelf, totalMax);
        }

        foreach (ITile tile in totalStack) {
            tile.xChainTotal = xMax;
            tile.yChainTotal = yMax;
            tile.totalChainTotal = totalMax;
            

            tile.isCalculated = false;

            //터지는 조건이 되면 폭발
            if(tile.totalChainTotal >= 3){
                tile.Blasted();
            }
        }

    }

    public override sealed void NearbyCheck(ref Stack<ITile> totalStack, Vector2 exceptionDirection){

        totalStack.Push(this);
        isCalculated = true;

        //초기화
        ChainClear();

        //X축 방향으로 검사
        foreach (var direction in Utils.xDirections){

            ITile tile = Raycast(direction, false);
            if (tile != null && !tile.isCalculated){
                this.xChainSelf = (ushort)Mathf.Max(tile.xChainSelf + 1, this.xChainSelf);
                this.totalChainSelf = (ushort)Mathf.Max(tile.totalChainSelf + 1, this.totalChainSelf);

                //재귀적으로
                if (direction != exceptionDirection) tile.NearbyCheck(ref totalStack, -direction);
            }
        }

        //Y축 방향으로 검사
        foreach (var direction in Utils.yDirections){
            if (direction == exceptionDirection) continue;

            ITile tile = Raycast(direction, false);
            if (tile != null && !totalStack.Contains(tile)){
                this.yChainSelf = (ushort)Mathf.Max(tile.yChainSelf + 1, this.yChainSelf);
                this.totalChainSelf = (ushort)Mathf.Max(tile.totalChainSelf + 1, this.totalChainSelf);

                //재귀적으로
                if (direction != exceptionDirection) tile.NearbyCheck(ref totalStack, -direction);
            }
        }
    }
    public sealed override void Blasted(){
        if(true){

        } else {
            
        }
    }
}
