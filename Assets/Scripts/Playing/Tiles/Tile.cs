using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour, ITile
{
    private Chain _xChain;
    private Chain _yChain;
    private Chain _totalChain;
    private bool _isCalculated;
    public ushort xChainSelf
    {
        get => _xChain.self;
        set => _xChain.self = value;
    }

    public ushort yChainSelf
    {
        get => _yChain.self;
        set => _yChain.self = value;
    }

    public ushort totalChainSelf
    {
        get => _totalChain.self;
        set => _totalChain.self = value;
    }
    

    public ushort xChainTotal
    {
        get => _xChain.total;
        set => _xChain.total = value;
    }

    public ushort yChainTotal
    {
        get => _yChain.total;
        set => _yChain.total = value;
    }

    public ushort totalChainTotal
    {
        get => _totalChain.total;
        set => _totalChain.total = value;
    }

    public bool isCalculated
    {
        get => _isCalculated;
        set => _isCalculated = value;
    }

    public void ChainClear(){
        this.xChainSelf = 0;
        this.yChainSelf = 0;
        this.totalChainSelf = 0;
        this.xChainTotal = 0;
        this.yChainTotal = 0;
        this.totalChainTotal = 0;
    }



    

    public void StartNearbyCheck(){
        Stack<ITile> totalStack = new Stack<ITile>();

        NearbyCheck(ref totalStack, Vector2.zero);

        ushort xMax = 0;
        ushort yMax = 0;
        ushort totalMax = 0;

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
        }

    }

    public void NearbyCheck(ref Stack<ITile> totalStack, Vector2 exceptionDirection){
        totalStack.Push(this);
        isCalculated = true;

        //초기화
        ChainClear();

        //X축 방향으로 검사
        foreach (var direction in Utils.xDirections){

            ITile tile = Raycast(direction);
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

            ITile tile = Raycast(direction);
            if (tile != null && !totalStack.Contains(tile)){
                this.yChainSelf = (ushort)Mathf.Max(tile.yChainSelf + 1, this.yChainSelf);
                this.totalChainSelf = (ushort)Mathf.Max(tile.totalChainSelf + 1, this.totalChainSelf);

                //재귀적으로
                if (direction != exceptionDirection) tile.NearbyCheck(ref totalStack, -direction);
            }
        }
    }

    

    private ITile Raycast(Vector2 direction){
        int myLayerMask = 1 << gameObject.layer;

        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, direction, Utils.RAYCAST_LENGHT, myLayerMask);

        return hit? hit.collider.gameObject.GetComponent<ITile>() : null;
    }
}
