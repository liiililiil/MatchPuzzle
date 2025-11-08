
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
//일반적으로 폭발할 때 생성하는 파괴자들
[Serializable]
public class ExplosionType
{
    public DestroyerType type;
    public Vector2 pos;
    public float rotate;
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
    StraightFlyEffect = 1,
    ShooterChildEffect = 2,
}

[Serializable]
public enum DestroyerType : ushort
{
    Straight = 0,
    Big = 1,
    Huge = 2,
    ShooterParent = 3,
}

[Serializable]
public enum DisabledType : ushort
{
    Blasted = 0,
    BlastedByDestroyer = 1,
    Disabled = 2,
    OnOrganized = 3,
    Extinction = 4,
    
}

/// <summary>
/// 상수처럼 사용됨 (실제 상수는 아님)
/// </summary>
public static class TILE_CONSTANT
{
    public static TileType[] COLOR_TILES = { TileType.Red, TileType.Green, TileType.Blue, TileType.Purple };
    public static TileType[] BOMB_TILES = { TileType.BigBomb, TileType.XBomb, TileType.YBomb, TileType.ColorBomb};
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

    // 재사용 가능한 리스트 버퍼
    private readonly List<Action> buffer = new List<Action>();

    public void Invoke()
    {
        // 버퍼 초기화
        buffer.Clear();

        // 해시셋 요소를 버퍼로 복사
        buffer.AddRange(hashSet);

        // 해시셋 초기화
        hashSet.Clear();

        // 액션 호출
        for (int i = 0; i < buffer.Count; i++)
        {
            buffer[i]?.Invoke();
        }

        // 버퍼는 재사용 가능 -> GC 발생 없음
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

    public int Count => hashSet.Count;
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

// 사용하지 않음
// 정확하게 연산하기 위해 이벤트 전과 후로 분리된 유니티 이벤트
// public class SplitUnityEvent<T>
// {
//     public UnityEvent<T> before = new UnityEvent<T>();
//     public UnityEvent<T> after = new UnityEvent<T>();
// }

// public class SplitUnityEvent
// {
//     public UnityEvent before = new UnityEvent();
//     public UnityEvent after = new UnityEvent();
// }
