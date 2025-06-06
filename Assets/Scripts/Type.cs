
using System;
using System.Collections.Generic;
using UnityEngine;

public class Chain
{
    private ushort _total;
    private ushort _self;
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
public enum TileType : ushort{
    Block = 0,
    Red = 1,
    Green = 2,
    Blue = 3,
    Purple = 4,
    BigBomb = 5,
    XBomb = 6,
    YBomb = 7,
    ColorBomb = 8,
    Box = 9

}
[Serializable]
public class TileData
{
    public GameObject prefab;
    public TileType tileType;
    public Queue<GameObject> pooling = new Queue<GameObject>();

}

public class ActionStack
{
    private readonly Stack<Action> stack = new Stack<Action>();
    private readonly HashSet<Action> hashSet = new HashSet<Action>();

    public void Invoke()
    {
        while (stack.Count > 0)
        {
            var action = stack.Pop();
            hashSet.Remove(action);
            action.Invoke();
        }
    }

    public void Push(Action action)
    {
        if (stack.Count >= 1000)
        {
            while (stack.Count > 0)
            {
                var _action = stack.Pop();
                hashSet.Remove(action);
            }

           throw new Exception("무한 루프 감지됨!"); 
        } 

        if (hashSet.Add(action))
            stack.Push(action);
    }

    public static ActionStack operator +(ActionStack actionStack, Action action)
    {
        actionStack.Push(action);
        return actionStack;
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
