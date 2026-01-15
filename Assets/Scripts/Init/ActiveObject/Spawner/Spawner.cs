using UnityEngine;
using System;

// 스폰 구조체
[Serializable]
public class SpawnRate
{
    public TileType tile;

    // 확률 가중치
    public float weight;
    
    // 실제 확률
    [HideInInspector]
    public float cumulativeWeight;
}


// 스포너 매인 클래스
public class Spawner : MonoBehaviour
{
    // 스폰 확률 구조체
    [SerializeField]
    private SpawnRate[] spawnTable;
    
    private void Start()
    {

        // 가중치를 기반으로 실제 확률 게산
        float max = 0;
        foreach (SpawnRate spawnRate in spawnTable)
            max += spawnRate.weight;
        

        float currentPoint = 0;
        foreach (SpawnRate spawnRate in spawnTable)
            spawnRate.cumulativeWeight = currentPoint += spawnRate.weight / max;

        // 스폰 이벤트 리스너 등록
        EventManager.Instance.InvokeSpawnTile.AddListener(TrySpawn);
    }
    
    // 스폰 시도 함수
    private void TrySpawn()
    {
        // 스포너 위치에 타일이 없을 때만 스폰
        Collider2D collider = Physics2D.OverlapBox(transform.position, Utils.FloatToVector2(Utils.TILE_SIZE), transform.rotation.z);

        if (collider == null || collider.GetComponent<Tile>() == null)
            SpawnManager.Instance.SpawnObject(GetTileFromTable(), transform.position, transform.rotation);
    }

    // 스폰 테이블에서 타일 타입을 가져오는 함수
    private TileType GetTileFromTable()
    {
        float random = UnityEngine.Random.value;

        foreach (SpawnRate spawnRate in spawnTable)
            if (random <= spawnRate.cumulativeWeight) return spawnRate.tile;

        throw new Exception("스폰 테이블이 없습니다!");
    }

    // 리스너 해제
    void OnDestroy()
    {
        EventManager.Instance.InvokeSpawnTile.RemoveListener(TrySpawn);
    }
}

