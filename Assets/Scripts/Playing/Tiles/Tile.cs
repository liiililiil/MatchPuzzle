using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour, ITile
{

    protected Chain _xChain;
    protected Chain _yChain;
    protected Chain _totalChain;
    protected bool _isCalculated;
    protected SpawnManager spawnManager;
    protected BoxCollider2D boxCollider2D;



    private Vector2 direction;
    public Chain xChain {get => _xChain; set => _xChain = value;}
    public Chain yChain {get => _yChain; set => _yChain = value;}
    public Chain totalChain {get => _totalChain; set => _totalChain = value;}
    public bool isCalculated {get => _isCalculated; set => _isCalculated = value;}

    public abstract TileType tileType { get; }

    public void ChainReset(){
        xChain.Reset();
        yChain.Reset();
        totalChain.Reset();
    
    }

    public void Bind(SpawnManager spawnManager){
        this.spawnManager = spawnManager;
    }
    protected ITile Raycast(Vector2 direction, int lenghtMultiple, bool isGlobal)
    {
        // 박스 콜라이더 지정
        if(boxCollider2D == null)
            boxCollider2D = GetComponent<BoxCollider2D>();

        // 자기 레이어만 감지하도록 설정
        int layerMask;

        // 레이어 마스크 설정
        if(isGlobal) layerMask = -1;
        else layerMask = 1 << gameObject.layer;

        // 로컬 기준 direction을 월드 방향으로 변환
        Vector2 worldDirection = transform.TransformDirection(direction);

        //사격전 자신은 제외시키기
        

        // Raycast 수행
        // Debug.Log("Raycast from: " + transform.position + " to: " +  worldDirection + " with layerMask: " + layerMask);
        boxCollider2D.enabled = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, worldDirection, Utils.RAYCAST_LENGHT * lenghtMultiple, layerMask);
        boxCollider2D.enabled = true;

        // Debug.Log("Raycast hit: " + hit.collider);


        // 결과 반환
        return hit ? hit.collider.GetComponent<ITile>() : null;
    }

    public  void ForceBlasted(){
        pooling();
    }

    public void pooling(){
        gameObject.transform.position = new Vector2(Utils.WAIT_POS_X,Utils.WAIT_Pos_Y);
        spawnManager.Pooling(gameObject, tileType);

    }
    
    // public abstract void MoveMent(Vector2 target);

    public abstract void Blasted();
    public abstract void Drop();
    public abstract void Calculate();
    public abstract void NearbyCheck(ref Stack<ITile> totalStack, Vector2 exceptionDirection);
    

    private void OnCollisionEnter(Collision collision){
        //자신은 강제 삭제
        ForceBlasted();

        //자신의 위 타일에게 낙하을 요청
        ITile tile = Raycast(Vector2.up, 1, true);
        tile?.Drop();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (new Vector3(1, 0) * Utils.RAYCAST_LENGHT));
        Gizmos.DrawLine(transform.position, transform.position + (new Vector3(-1, 0) * Utils.RAYCAST_LENGHT));
        Gizmos.DrawLine(transform.position, transform.position + (new Vector3(0, 1) * Utils.RAYCAST_LENGHT));
        Gizmos.DrawLine(transform.position, transform.position + (new Vector3(0, -1) * Utils.RAYCAST_LENGHT));
        Gizmos.DrawCube(transform.position, new Vector3(Utils.TILE_SIZE/4, Utils.TILE_SIZE/4, 1));
    }
}
