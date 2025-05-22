using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


[System.Serializable]
public class TileData
{
    public GameObject prefab;
    public TileType tileType;
    public Queue<GameObject> pooling;
    
}
public class SpawnManager : MonoBehaviour{
    [SerializeField]
    private List<TileData> tileData;
    private TileData[] tileDataIndex = new TileData[Utils.TILETYPE_LENGHT+1];



    void Awake()
    {
        //다시 정렬
        tileData.Sort((a, b) => a.tileType.CompareTo(b.tileType));

        ushort point = 0;

        //인덱스 해놓기
        for (ushort i = 0; i < Utils.TILETYPE_LENGHT + 1; i++)
        {
            if (point >= tileData.Count) break;
            
            if ((ushort)tileData[point].tileType == i)
            {
                tileDataIndex[i] = tileData[point];
                point++;
            }
        }
    }

    private GameObject GetTile(TileData tileData, Vector2 position, Quaternion rotate){
        if(tileData.pooling.Count <= 0) return Instantiate(tileData.prefab, position, rotate);

        GameObject gameObject = tileData.pooling.Dequeue();

        gameObject.transform.position = position;
        gameObject.transform.rotation = rotate;

        return gameObject; 
    }

    public void SpawnTile(TileType tileType, Vector2 position, Quaternion rotate){
       GetTile(tileDataIndex[(ushort)tileType], position, rotate).GetComponent<ITile>()?.Bind(this);
    }

    public void Pooling(GameObject gameObject, TileType tileType){
        tileDataIndex[(ushort)tileType].pooling.Enqueue(gameObject);
    }

}
