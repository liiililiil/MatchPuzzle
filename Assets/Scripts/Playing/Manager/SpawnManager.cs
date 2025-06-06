using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using UnityEngine;


public class SpawnManager : MonoBehaviour{
    public static SpawnManager Instance { get; private set; }

    [SerializeField]
    private TileData[] tileData;

    private TileData[] tileDataIndex;



    void Awake(){
        tileDataIndex = new TileData[Utils.TILETYPE_LENGHT];
        
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

            // Debug.Log(data.tileType);

            if (tileDataIndex[type] == null) tileDataIndex[type] = data;
        }
    }

    private GameObject GetTile(TileData tileData)
    {
        if (tileData.pooling.Count <= 0) return Instantiate(tileData.prefab);
        return tileData.pooling.Dequeue();
    }
    public void pooling(GameObject gameObject, ITile tile){
        tileDataIndex[(ushort)tile.tileType].pooling.Enqueue(gameObject);
    }
    public void SpawnTile(TileType tileType, Vector2 position, Quaternion rotate)
    {
        GameObject gameObject = GetTile(tileDataIndex[(ushort)tileType]);
        // Debug.Log("Go");

        ITile itile = gameObject.GetComponent<ITile>();
        // Debug.Log("tile");
        itile.Enable(position, rotate);
    }

}
