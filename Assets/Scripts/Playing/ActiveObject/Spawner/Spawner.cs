using Unity.Mathematics;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private SpawnRate[] spawnTable;
    private void Start()
    {
        EventManager.Instance.OnSpawnTile += TrySpawn;

        //확률 구하기

        //일단 값을 찾고
        float max = 0;
        foreach (SpawnRate spawnRate in spawnTable)
        {
            max += spawnRate.weight;
        }

        float currentPoint = 0;
        foreach (SpawnRate spawnRate in spawnTable)
        {
            spawnRate.cumulativeWeight = currentPoint += spawnRate.weight / max;
        }
        
    }

    private void TrySpawn()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);

        if (raycastHit2D.collider == null || raycastHit2D.collider.GetComponent<ITile>() == null)
            SpawnManager.Instance.SpawnTile(GetTileFromTable(), gameObject.transform.position, Quaternion.identity);
    }

    private TileType GetTileFromTable()
    {
        float random = UnityEngine.Random.value;

        foreach (SpawnRate spawnRate in spawnTable)
        {
            if (random <= spawnRate.cumulativeWeight) return spawnRate.tile;
        }
        throw new System.Exception("스폰 테이블이 없습니다!");
    }
    
    ~Spawner()
    {
        try
        {
            EventManager.Instance.OnSpawnTile -= TrySpawn;
        }
        catch
        {
            Debug.LogWarning("스포너가 등록되지 않은 상태에서 소멸이 확인되었습니다!");
        }
    }
}

