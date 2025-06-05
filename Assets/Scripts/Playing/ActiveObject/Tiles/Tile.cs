using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Tile : MonoBehaviour, ITile
{
    public Chain _xChain = new Chain();
    public Chain _yChain = new Chain();
    public Chain _totalChain = new Chain();
    protected byte bitFlag;
    protected CircleCollider2D circleCollider2D;
    protected new Rigidbody2D rigidbody2D;
    public Chain xChain { get => _xChain; set => _xChain = value; }
    public Chain yChain { get => _yChain; set => _yChain = value; }
    public Chain totalChain { get => _totalChain; set => _totalChain = value; }
    public bool isCalculated { get => (bitFlag & 1 << 0) != 0; set => bitFlag = (byte)(value ? bitFlag | 1 << 0 : bitFlag & ~(1 << 0)); }
    public bool isDrop { get => (bitFlag & 1 << 1) != 0; set => bitFlag = (byte)(value ? bitFlag | 1<<1 : bitFlag & ~(1<<1)); }

    public void ChainReset()
    {
        xChain.Reset();
        yChain.Reset();
        totalChain.Reset();

    }

    public void CalReset()
    {
        try
        {
            EventManager.Instance.OnCalReset -= CalReset;

        }
        catch
        {
            Debug.LogWarning("독립적으로 실행되었습니다!");
        }

        isCalculated = false;

        ChainReset();
    }

    protected ITile Raycast(Vector2 direction, float lenghtMultiple, bool isGlobal)
    {
        int layerMask;
        if (isGlobal) layerMask = ~0;
        else layerMask = 1 << gameObject.layer;

        Vector2 worldDirection = transform.TransformDirection(direction).normalized;

        // 콜라이더 바깥으로 Raycast 시작 지점 계산
        Vector2 start = (Vector2)transform.position + worldDirection * Utils.RAYCASY_REVISION;

        // Raycast 수행
        Debug.DrawRay(start, worldDirection * (Utils.RAYCASY_LENGHT * lenghtMultiple), Color.red, 0.1f);
        RaycastHit2D hit = Physics2D.Raycast(start, worldDirection, Utils.RAYCASY_LENGHT * lenghtMultiple, layerMask);
        // Debug.Log(hit.collider.name

        return hit.collider ? hit.collider.GetComponent<ITile>() : null;
    }


    public void ForceBlasted()
    {
        Disable();
    }

    public void Disable()
    {
        try
        {
            EventManager.Instance.OnDisableTile.Invoke(this, transform.position);
        }
        catch
        {
            Debug.LogWarning("타일 비활성화 액션에 구독된 함수가 하나도 없습니다!");
        }

        //위 타일에게 낙하를 명령

        if (circleCollider2D == null) circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.enabled = false;

        ITile[] aboveTile = { Raycast(Vector2.up, 1, true), Raycast(Vector2.up + Vector2.right, 1.7f, true), Raycast(Vector2.up + Vector2.left, 1.7f, true) };
        foreach (ITile tile in aboveTile) tile?.Drop();
        
        gameObject.transform.position = new Vector2(Utils.WAIT_POS_X, Utils.WAIT_Pos_Y);
        SpawnManager.Instance.pooling(gameObject, this);
    }

    public void Enable(Vector2 pos, quaternion rotate)
    {
        circleCollider2D.enabled = true;
        gameObject.transform.position = pos;
        gameObject.transform.rotation = rotate;

        EventManager.Instance.OnSpawnTile(this, transform.position);
    }

    // public abstract void MoveMent(Vector2 target);

    public abstract void Blasted();
    public abstract void Calculate();
    public abstract void NearbyCheck(ref Stack<ITile> totalStack, Vector2 exceptionDirection);
    public abstract void Drop();
    public abstract void Organize();


    private void OnCollisionEnter(Collision collision)
    {
        //충돌한 대상의 파괴자 가져오기
        ITileDestroyer tileDestroyer = collision.gameObject.GetComponent<ITileDestroyer>();

        if(tileDestroyer == null) Debug.LogError("파괴자가 아닌 대상과 충돌하였습니다!");

        EventManager.Instance.OnBlastTileByBomb(this, tileDestroyer, transform.position);
        
        Disable();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position, Utils.FloatToVector2(Utils.TILE_SIZE));
    }
    
    
    
    
}
