using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;


[Serializable]
public class IndexData<T> where T : Enum
{
    [SerializeField]
    private PoolableData<T>[] data;
    [HideInInspector]
    public PoolableData<T>[] index;

    public void Indexing() 
    {
        index = new PoolableData<T>[Enum.GetValues(typeof(T)).Length];

        foreach (var item in data)
        {
            ushort type = (ushort)(object)item.type;

            if (index[type] == null) index[type] = item;
            else
            {
                Debug.LogError($"중복된 타입: {item.type}");
            }
        }

    }
}
public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [SerializeField]
    private IndexData<TileType> tileData;

    [SerializeField]
    private IndexData<DestroyerType> destroyerData;
    [SerializeField]
    private IndexData<EffectType> effectData;




    private Dictionary<Type, object> indexMap = new();



    void Awake()
    {
        indexMap[typeof(TileType)] = tileData;
        indexMap[typeof(DestroyerType)] = destroyerData;
        indexMap[typeof(EffectType)] = effectData;

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
        tileData.Indexing();
        destroyerData.Indexing();
        effectData.Indexing();

        SpawnObject(DestroyerType.Straight, Vector2.zero, Quaternion.identity);

    }

    private GameObject GetObject<T>(T type) where T : Enum
    {
        PoolableData<T>[] data;
        TryGetData(out data);


        int idx = Convert.ToInt32(type);

        if (data == null)
        {
            Debug.LogError($"잘못된 인덱스 접근: {idx} for {typeof(T).Name}");
            return null;
        }

        if (data[idx].pooling.Count > 0)
            return data[idx].pooling.Dequeue();

        return Instantiate(data[idx].prefab);
    }

    private void TryGetData<T>(out PoolableData<T>[] data) where T : Enum
    {
        data = null;
        if (typeof(T) == typeof(TileType))
            data = tileData.index as PoolableData<T>[];
        else if (typeof(T) == typeof(DestroyerType))
            data = destroyerData.index as PoolableData<T>[];
        else if (typeof(T) == typeof(EffectType))
            data = effectData.index as PoolableData<T>[];
    }
    public void Pooling<T>(T type, GameObject gameObject) where T : Enum
    {
        PoolableData<T>[] data;
        TryGetData(out data);

        int idx = Convert.ToInt32(type);

        if (data != null)
            data[idx].pooling.Enqueue(gameObject);
    }

    public void SpawnObject<T>(T type, Vector2 position, Quaternion rotate, IActiveObject caller = null) where T : Enum
    {
        GameObject gameObject = GetObject(type);
        
        // Debug.Log(gameObject);
        gameObject.GetComponent<IActiveObject>().Enable(position, rotate, caller);
    }

    

}
