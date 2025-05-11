using UnityEngine;


[System.Serializable]
public struct TileData
{
    public GameObject prefab;
    public TileType tileType;
    
}
public class SpawnManager : MonoBehaviour{
    [SerializeField]
    private TileData[] tileData;

    private GameObject TryGetPrefab(TileType tileType){
        foreach (TileData tile in tileData){
            if (tile.tileType == tileType)
                return tile.prefab;
        }

        return null;
    }

    public void SpawnTile(TileType tileType, Vector2 position, Quaternion rotate){
        GameObject prefab = TryGetPrefab(tileType);
        
        if (prefab != null){
            GameObject tileGameObject = Instantiate(prefab, position, rotate);
            ITile tile = tileGameObject.GetComponent<ITile>();

            tile.Bind(this);

        }
    }

    public void SpawnGameObject(GameObject gameObject, Transform transform, Vector2 roatate){
        
    }
}
