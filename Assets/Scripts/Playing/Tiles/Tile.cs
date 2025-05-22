using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour, ITile
{


    protected Chain _xChain = new Chain();
    protected Chain _yChain = new Chain();
    protected Chain _totalChain = new Chain();
    protected bool _isCalculated;
    protected SpawnManager spawnManager;
    protected BoxCollider2D boxCollider2D;
    protected Rigidbody2D rigidbody2D;



    private Vector2 direction;
    public Chain xChain { get => _xChain; set => _xChain = value; }
    public Chain yChain { get => _yChain; set => _yChain = value; }
    public Chain totalChain { get => _totalChain; set => _totalChain = value; }
    public bool isCalculated { get => _isCalculated; set => _isCalculated = value; }

    public abstract TileType tileType { get; }

    public void ChainReset()
    {
        xChain.Reset();
        yChain.Reset();
        totalChain.Reset();

    }

    public void Bind(SpawnManager spawnManager)
    {
        this.spawnManager = spawnManager;
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    protected ITile Raycast(Vector2 direction, int lenghtMultiple, bool isGlobal)
    {
        if (boxCollider2D == null)
            boxCollider2D = GetComponent<BoxCollider2D>();

        int layerMask;
        if (isGlobal) layerMask = -1;
        else layerMask = 1 << gameObject.layer;

        Vector2 worldDirection = transform.TransformDirection(direction).normalized;

        // 콜라이더 바깥으로 Raycast 시작 지점 계산
        Vector2 start = (Vector2)transform.position + worldDirection * Utils.RAYCASY_REVISION;

        // Raycast 수행
        Debug.DrawRay(start, worldDirection * (Utils.RAYCASY_LENGHT * lenghtMultiple), Color.red, 0.1f);
        RaycastHit2D hit = Physics2D.Raycast(start, worldDirection, Utils.RAYCASY_LENGHT * lenghtMultiple, layerMask);

        return hit ? hit.collider.GetComponent<ITile>() : null;
    }


    public void ForceBlasted()
    {
        pooling();
    }

    public void pooling()
    {
        gameObject.transform.position = new Vector2(Utils.WAIT_POS_X, Utils.WAIT_Pos_Y);
        spawnManager.Pooling(gameObject, tileType);

    }

    // public abstract void MoveMent(Vector2 target);

    public abstract void Blasted();
    public abstract void Drop();
    public abstract void Calculate();
    public abstract void NearbyCheck(ref Stack<ITile> totalStack, Vector2 exceptionDirection);


    private void OnCollisionEnter(Collision collision)
    {
        //자신은 강제 삭제
        ForceBlasted();

        //자신의 위 타일에게 낙하을 요청
        ITile tile = Raycast(Vector2.up, 1, true);
        tile?.Drop();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, Utils.FloatToVector2(Utils.TILE_SIZE));
    }
    
    
}
