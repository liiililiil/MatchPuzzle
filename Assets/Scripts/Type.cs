
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
    TileDisabled = 0,
    StraightFlyEffect = 1
}

[Serializable]
public enum DestroyerType : ushort
{
    Straight = 0,
    Big = 1,
    Color = 2
}


[Serializable]
public class PoolableData<T> where T : Enum
{
    public GameObject prefab;
    public T type;
    public Queue<GameObject> pooling = new Queue<GameObject>();
}

public class OneTimeAction
{
    private readonly HashSet<Action> hashSet = new HashSet<Action>();

    public void Invoke()
    {
        Action[] tempSet = hashSet.ToArray();
        hashSet.Clear();

        foreach (Action action in tempSet)
        {
            action?.Invoke();
        }

    }

    public void Add(Action action)
    {
        hashSet.Add(action);
    }

    public void Clear()
    {
        hashSet.Clear();
    }

    public static OneTimeAction operator +(OneTimeAction oneTimeAction, Action action)
    {
        oneTimeAction.Add(action);
        return oneTimeAction;
    }
    

    public int Count { get => hashSet.Count; }
}

public class OneTimeAction<T>
{
    private readonly HashSet<Action<T>> hashSet = new HashSet<Action<T>>();
    private readonly List<(Action<T> action, T args)> actionList = new List<(Action<T>, T)>();

    public void Addlistener(Action<T> action, T args)
    {
        if (hashSet.Add(action))
        {
            actionList.Add((action, args));
        }
    }

    public void Invoke()
    {
        foreach (var (action, args) in actionList)
        {
            action?.Invoke(args);
        }

        actionList.Clear();
        hashSet.Clear();
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


