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


    private Vector2 direction;
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
        EventManager.Instance.OnCalReset.RemoveListener(CalReset);
        isCalculated = false;
    }

    protected ITile Raycast(Vector2 direction, float lenghtMultiple, bool isGlobal)
    {
        int layerMask;
        if (isGlobal) layerMask = -1;
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
        circleCollider2D.enabled = false;
        gameObject.transform.position = new Vector2(Utils.WAIT_POS_X, Utils.WAIT_Pos_Y);
        SpawnManager.Instance.pooling(gameObject, this);
    }

    public void Enable(Vector2 pos, quaternion rotate)
    {
        circleCollider2D.enabled = true;
        gameObject.transform.position = pos;
        gameObject.transform.rotation = rotate;
    }

    // public abstract void MoveMent(Vector2 target);

    public abstract void Blasted();
    public abstract void Calculate();
    public abstract void NearbyCheck(ref Stack<ITile> totalStack, Vector2 exceptionDirection);
    public abstract void Drop();
    public abstract void Organize();


    private void OnCollisionEnter(Collision collision)
    {
        //자신은 강제 삭제
        ForceBlasted();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position, Utils.FloatToVector2(Utils.TILE_SIZE));
    }
    
    
    
    
}
