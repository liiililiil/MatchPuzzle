using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using UnityEngine;


public class SpawnManager : MonoBehaviour{
    public static SpawnManager Instance { get; private set; }

    [SerializeField]
    private List<TileData> tileData;

    private TileData[] tileDataIndex = new TileData[Utils.TILETYPE_LENGHT + 1];



    void Awake(){
        
        //싱글톤
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }
    
    private void Start() {
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

    private GameObject GetTile(TileData tileData, Vector2 position, Quaternion rotate)
    {
        if (tileData.pooling.Count <= 0) return Instantiate(tileData.prefab, position, rotate);

        GameObject gameObject = tileData.pooling.Dequeue();

        gameObject.transform.position = position;
        gameObject.transform.rotation = rotate;

        return gameObject;
    }
    public void SpawnTile(TileType tileType, Vector2 position, Quaternion rotate)
    {
        GetTile(tileDataIndex[(ushort)tileType], position, rotate);
    }
    private int GetTileType(ITile tile)
    {

        if (tile is Block) return 0;
        if (tile is Red) return 1;

        Debug.LogError("지정되지 않은 타일!");
        return 0;
    }

    public void Pooling(GameObject gameObject, ITile tile){
        tileDataIndex[GetTileType(tile)].pooling.Enqueue(gameObject);
    }

}
