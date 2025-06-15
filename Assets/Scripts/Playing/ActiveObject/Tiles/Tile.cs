using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Tile : MonoBehaviour, ITile
{
    protected Chain _xChain = new Chain();
    protected Chain _yChain = new Chain();

    // [SerializeField]
    // public Chain _totalChain = new Chain();
    public abstract TileType tileType { get; }
    public abstract bool isCanDrop { get; }
    protected byte bitFlag;
    protected BoxCollider2D boxCollider2D;
    protected GameObject sprite;
    protected Coroutine coroutine;
    public Chain xChain { get => _xChain; set => _xChain = value; }
    public Chain yChain { get => _yChain; set => _yChain = value; }
    // public Chain totalChain { get => _totalChain; set => _totalChain = value; }
    public bool isCalculated { get => (bitFlag & 1 << 0) != 0; set => bitFlag = (byte)(value ? bitFlag | 1 << 0 : bitFlag & ~(1 << 0)); }
    public bool isDrop { get => (bitFlag & 1 << 1) != 0; set => bitFlag = (byte)(value ? bitFlag | 1 << 1 : bitFlag & ~(1 << 1)); }
    // public bool needCallDrop { get => (bitFlag & 1 << 2) != 0; set => bitFlag = (byte)(value ? bitFlag | 1 << 2 : bitFlag & ~(1 << 2)); }

    public bool isCoroutineRunning()
    {
        if (coroutine != null) return true;
        return false;
    }



    public void ChainReset()
    {
        xChain.Reset();
        yChain.Reset();
        // totalChain.Reset();

    }

    public void CalReset()
    {
        isCalculated = false;

        ChainReset();
    }

    protected ITile GetTileFromWorld(Vector2 direction, bool isGlobal = false)
    {
        return GetTileFromWorld(direction, 1, isGlobal);
    }

    protected ITile GetTileFromWorld(Vector2 direction, int revisionMultiple, bool isGlobal = false)
    {
        int layerMask = isGlobal ? ~0 : 1 << gameObject.layer;

        Vector2 worldDirection = transform.TransformDirection(direction);

        // 콜라이더 바깥으로 GetTileFromWorld 시작 지점 계산
        Vector2 start = (Vector2)transform.position + worldDirection * Utils.RAYCASY_REVISION * revisionMultiple;

        // GetTileFromWorld 수행
        #if UNITY_EDITOR
        DrawOverlapBox(start,Utils.FloatToVector2(Utils.TILE_SIZE), transform.rotation.z, Color.red);
        #endif
        Collider2D hit = Physics2D.OverlapBox(start, Utils.FloatToVector2(Utils.TILE_SIZE), transform.rotation.z, layerMask);

        // Debug.Log(hit.collider.name
        return hit ? hit.GetComponent<ITile>() : null;
    }



    public void ForceBlasted()
    {
        Disable();
    }

    public void Disable()
    {

        EventManager.Instance.OnDisabledTile.Invoke(this, transform.position);

        if (boxCollider2D == null) boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;

        if (sprite == null) sprite = transform.GetChild(0).gameObject;
        sprite.SetActive(false);

        //위 타일에게 낙하를 명령
        ITile[] aboveTile = { GetTileFromWorld(Vector2.up, true), GetTileFromWorld(Vector2.up + Vector2.right, true), GetTileFromWorld(Vector2.up + Vector2.left, true) };
        foreach (ITile tile in aboveTile) tile?.Drop();

        // if (isDrop)
        // {
        //     isDrop = false;
        //     EventManager.Instance.movingTiles--;
        // }

        gameObject.transform.position = new Vector2(Utils.WAIT_POS_X, Utils.WAIT_Pos_Y);
        SpawnManager.Instance.pooling(gameObject, this);
    }

    public void Enable(Vector2 pos, quaternion rotate)
    {
        if (boxCollider2D == null) boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = true;

        if (sprite == null) sprite = transform.GetChild(0).gameObject;
        sprite.SetActive(true);

        CalReset();

        transform.position = pos;
        transform.rotation = rotate;

        EventManager.Instance.OnSpawnedTile.Invoke(this, transform.position);

        Drop();
    }

    // public abstract void MoveMent(Vector2 target);

    public abstract void Blasted(bool isCenter = false);
    public abstract void Calculate();
    public abstract void NearbyCheck(ref Stack<ITile> totalStack, Vector2 exceptionDirection, ushort chain);
    public abstract void Drop();
    public abstract void Organize();


    private void OnCollisionEnter(Collision collision)
    {
        //충돌한 대상의 파괴자 가져오기
        ITileDestroyer tileDestroyer = collision.gameObject.GetComponent<ITileDestroyer>();

        if (tileDestroyer == null) Debug.LogError("파괴자가 아닌 대상과 충돌하였습니다!");

        EventManager.Instance.OnBlastedTileByBomb.Invoke(this, tileDestroyer, transform.position);

        Disable();
    }

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawCube(transform.position, Utils.FloatToVector2(Utils.TILE_SIZE));
    // }


#if UNITY_EDITOR
    void DrawOverlapBox(Vector2 center, Vector2 size, float angleDeg, Color color, float duration = 0.05f)
    {
        float angleRad = angleDeg * Mathf.Deg2Rad;
        Vector2 right = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        Vector2 up = new Vector2(-right.y, right.x);

        Vector2 extentsX = right * (size.x / 2f);
        Vector2 extentsY = up * (size.y / 2f);

        Vector2 topRight = center + extentsX + extentsY;
        Vector2 topLeft = center - extentsX + extentsY;
        Vector2 bottomLeft = center - extentsX - extentsY;
        Vector2 bottomRight = center + extentsX - extentsY;

        Debug.DrawLine(topLeft, topRight, color, duration);
        Debug.DrawLine(topRight, bottomRight, color, duration);
        Debug.DrawLine(bottomRight, bottomLeft, color, duration);
        Debug.DrawLine(bottomLeft, topLeft, color, duration);
    }
#endif




}
