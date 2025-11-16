using System;
using System.Collections.Generic;
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
public class SpawnManager : Managers<SpawnManager>
{
    [SerializeField]
    private IndexData<TileType> tileData;

    [SerializeField]
    private IndexData<DestroyerType> destroyerData;
    [SerializeField]
    private IndexData<EffectType> effectData;
    private void Start()
    {
        tileData.Indexing();
        destroyerData.Indexing();
        effectData.Indexing();
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
        {
            return VerificationDeQueue(ref data[idx].pooling);
        }

        return Instantiate(data[idx].prefab);
    }

    // 재귀호출로 null인 오브젝트를 걸러냄
    private GameObject VerificationDeQueue(ref Queue<GameObject> queue)
    {
        GameObject dequqeued = queue.Dequeue();

        if (dequqeued == null)
        {
            return VerificationDeQueue(ref queue);
        }
        else
        {
            return dequqeued;
        }
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

    public GameObject SpawnObject<T>(T type, Vector2 position, Quaternion rotate, IActiveObject caller = null) where T : Enum
    {
        GameObject gameObject = GetObject(type);

        gameObject.GetComponent<IActiveObject>().Enable(position, rotate, caller);

        return gameObject;
    }

    

}
