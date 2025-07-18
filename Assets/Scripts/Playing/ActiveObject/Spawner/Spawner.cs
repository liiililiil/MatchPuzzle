using Unity.Mathematics;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private SpawnRate[] spawnTable;
    private void Start()
    {

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
        
        EventManager.Instance.InvokeSpawnTile.AddListener(TrySpawn);
    }

    private void TrySpawn()
    {
        Collider2D collider = Physics2D.OverlapBox(transform.position, Utils.FloatToVector2(Utils.TILE_SIZE), transform.rotation.z);

        if (collider == null || collider.GetComponent<Tile>() == null)
            SpawnManager.Instance.SpawnObject(GetTileFromTable(), transform.position, transform.rotation);
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
        EventManager.Instance.InvokeSpawnTile.RemoveListener(TrySpawn);
    }
}

