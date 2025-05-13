using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct TileData
{
    public GameObject prefab;
    public TileType tileType;
    public Queue<GameObject> pooling;
    
}
public class SpawnManager : MonoBehaviour{
    [SerializeField]
    private TileData[] tileData;

    private int TryGetTileRow(TileType tileType){
        for (int i = 0; i < tileData.Length; i++)
        {
            if (tileData[i].tileType == tileType)
            return i;
        }

        throw new System.Exception($"TileData Not Found! : {tileData}");
    }

    private GameObject GetTile(int row, Vector2 position, Quaternion rotate){
        if(tileData[row].pooling.Count <= 0) return Instantiate(tileData[row].prefab, position, rotate);

        GameObject gameObject = tileData[row].pooling.Dequeue();

        gameObject.transform.position = position;
        gameObject.transform.rotation = rotate;

        return gameObject; 
    }

    public void SpawnTile(TileType tileType, Vector2 position, Quaternion rotate){
       GetTile(TryGetTileRow(tileType), position, rotate).GetComponent<ITile>()?.Bind(this);
    }

    public void Pooling(GameObject gameObject, TileType tileType){
        tileData[TryGetTileRow(tileType)].pooling.Enqueue(gameObject);
    }

}
