using System;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using UnityEngine;


public class SpawnManager : MonoBehaviour{
    public static SpawnManager Instance { get; private set; }

    [SerializeField]
    private List<TileData> tileData;

    private TileData[] tileDataIndex;



    void Awake(){
        tileDataIndex = new TileData[tileData.Count + 1];
        
        //싱글톤
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {

        //인덱싱
        foreach (var data in tileData)
        {
            ushort type = (ushort)data.tileType;

            if (tileDataIndex[type] == null) tileDataIndex[type] = data;
        }
    }

    private GameObject GetTile(TileData tileData)
    {
        if (tileData.pooling.Count <= 0) return Instantiate(tileData.prefab);
        return tileData.pooling.Dequeue();
    }
    public void SpawnTile(TileType tileType, Vector2 position, Quaternion rotate)
    {
        GameObject gameObject = GetTile(tileDataIndex[(ushort)tileType]);
        gameObject.GetComponent<ITile>()?.Enable(position, rotate);
    }
    private int GetTileType(ITile tile)
    {

        if (tile is Block) return 0;
        if (tile is Red) return 1;

        Debug.LogError("지정되지 않은 타일!");
        return 0;
    }

    public void pooling(GameObject gameObject, ITile tile){
        tileDataIndex[GetTileType(tile)].pooling.Enqueue(gameObject);
    }

}
