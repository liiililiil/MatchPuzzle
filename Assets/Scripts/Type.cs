
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Chain
{
    public ushort _total;
    public ushort _self;
    public ushort self
    {
        get => _self;
        set => _self = value;
    }

    public ushort total
    {
        get => _total;
        set => _total = value;
    }

    public void Reset()
    {
        _self = 0;
        _total = 0;
    }

    public static Chain operator +(Chain a, Chain b)
    {
        return new Chain
        {
            self = (ushort)(a.self + b.self),
            total = (ushort)(a.total + b.total)
        };
    }
}

[Serializable]
public enum TileType : ushort
{
    Block = 0,
    Red = 1,
    Green = 2,
    Blue = 3,
    Purple = 4,
    BigBomb = 5,
    XBomb = 6,
    YBomb = 7,
    ColorBomb = 8,
    Box = 9,
    Empty = 10,
}
[Serializable]
public enum EffectType : ushort
{
    Disabled = 0
}

[Serializable]
public enum DestroyerType : ushort
{
    Straight = 0,
    Big = 1,
    Color = 2,
}


[Serializable]
public class PoolableData<T> where T : Enum
{
    public GameObject prefab;
    public T type;
    public Queue<GameObject> pooling = new Queue<GameObject>();

    public static implicit operator PoolableData<T>(IndexData<TileType> v)
    {
        throw new NotImplementedException();
    }
}


public class OneTimeAction
{
    private readonly HashSet<Action> hashSet = new HashSet<Action>();

    public void Invoke()
    {
        Action[] tempSet = hashSet.ToArray();
        foreach (Action action in tempSet)
        {
            action?.Invoke();
        }

        hashSet.Clear();
    }

    public void Add(Action action)
    {
        hashSet.Add(action);
    }

    public static OneTimeAction operator +(OneTimeAction oneTimeAction, Action action)
    {
        oneTimeAction.Add(action);
        return oneTimeAction;
    }
}

// // 클리어 조건 구조
// [System.Serializable]
// public struct StageGoal
// {
//     TileBurstGoal[] tileBurstGoals;
//     int movementLimit;
//     float time;
//     float extraTime;
// }

// [System.Serializable]
// public struct TileBurstGoal
// {
//     TileType type;
//     int goalCount;
// }

[Serializable]
public class SpawnRate
{
    public TileType tile;
    public float weight;

    [HideInInspector]
    public float cumulativeWeight;
}

// [System.Serializable]
// public struct TileStartInfo
// {
//     TileType tile;
    
//     Vector2 pos;

// }
