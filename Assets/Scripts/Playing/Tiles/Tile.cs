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

    public ushort xChainSelf { get => _xChain.self; set => _xChain.self = value; } 
    public ushort yChainSelf { get => _yChain.self; set => _yChain.self = value; } 
    public ushort totalChainSelf { get => _totalChain.self; set => _totalChain.self = value; } 
    public ushort xChainTotal { get => _xChain.total; set => _xChain.total = value; } 
    public ushort yChainTotal { get => _yChain.total; set => _yChain.total = value; } 
    public ushort totalChainTotal { get => _totalChain.total; set => _totalChain.total = value; } 
    public bool isCalculated { get => _isCalculated; set => _isCalculated = value; }


    public void ChainClear(){
        this.xChainSelf = 0;
        this.yChainSelf = 0;
        this.totalChainSelf = 0;
        this.xChainTotal = 0;
        this.yChainTotal = 0;
        this.totalChainTotal = 0;
    }

    public void Bind(SpawnManager spawnManager){
        this.spawnManager = spawnManager;
    }
    public abstract void Calculate();
    public abstract void NearbyCheck(ref Stack<ITile> totalStack, Vector2 exceptionDirection);
    

    protected ITile Raycast(Vector2 direction, bool isGlobal)
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, worldDirection, Utils.RAYCAST_LENGHT, layerMask);
        boxCollider2D.enabled = true;

        // Debug.Log("Raycast hit: " + hit.collider);


        // 결과 반환
        return hit ? hit.collider.GetComponent<ITile>() : null;
    }

    public  void ForceBlasted(){
        Destroy(this);

    }

    public abstract void Blasted();
    public abstract void Drop();

    private void OnCollisionEnter(Collision collision){
        //자신은 강제 삭제
        ForceBlasted();

        //자신의 위 타일에게 낙하을 요청
        ITile tile = Raycast(Vector2.up, true);
        tile?.Drop();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (new Vector3(1, 0) * Utils.RAYCAST_LENGHT));
        Gizmos.DrawLine(transform.position, transform.position + (new Vector3(-1, 0) * Utils.RAYCAST_LENGHT));
        Gizmos.DrawLine(transform.position, transform.position + (new Vector3(0, 1) * Utils.RAYCAST_LENGHT));
        Gizmos.DrawLine(transform.position, transform.position + (new Vector3(0, -1) * Utils.RAYCAST_LENGHT));
        Gizmos.DrawCube(transform.position, new Vector3(Utils.TILE_SIZE, Utils.TILE_SIZE, 1));
    }
}
