
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
            //이미 파괴 되어 있는 경우 처리
            try
            {
                buffer[i]?.Invoke();
            }
            catch (MissingReferenceException)
            {
                continue;
            }
        }
    }

    public void Add(Action action)
    {
        hashSet.Add(action);
    }

    public void Remove(Action action)
    {
        hashSet.Remove(action);
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

    public int getCount()
    {
        return Mathf.Max(hashSet.Count, buffer.Count);
    }
}

public class SimpleEvent
{
    private readonly List<Action> actionList = new List<Action>();

    public void AddListener(Action action)
    {
        actionList.Add(action);
    }
    public void RemoveListener(Action action)
    {
        actionList.Remove(action);
    }

    public void Invoke()
    {
        for (int i = actionList.Count - 1; i >= 0; i--)
        {
            var action = actionList[i];
            try
            {
                action?.Invoke();
            }
            catch (MissingReferenceException)
            {
                actionList.RemoveAt(i);
            }
        }
    }

}

public class SimpleEvent<T>
{
    private readonly List<Action<T>> actionList = new List<Action<T>>();

    public void AddListener(Action<T> action)
    {
        actionList.Add(action);
    }

    public void RemoveListener(Action<T> action)
    {
        actionList.Remove(action);
    }

    public void Invoke(T arg)
    {
        for (int i = actionList.Count - 1; i >= 0; i--)
        {
            var action = actionList[i];
            try
            {
                action?.Invoke(arg);
            }
            catch (MissingReferenceException)
            {
                actionList.RemoveAt(i);
            }
        }
    }
}

public class SimpleEvent<T1, T2>
{
    private readonly List<Action<T1, T2>> actionList = new List<Action<T1, T2>>();

    public void AddListener(Action<T1, T2> action)
    {
        actionList.Add(action);
    }

    public void RemoveListener(Action<T1, T2> action)
    {
        actionList.Remove(action);
    }

    public void Invoke(T1 arg1, T2 arg2)
    {
        for (int i = actionList.Count - 1; i >= 0; i--)
        {
            var action = actionList[i];
            try
            {
                action?.Invoke(arg1, arg2);
            }
            catch (MissingReferenceException)
            {
                actionList.RemoveAt(i);
            }
        }
    }
}
public class StagePrograss
{
    public int score;
    public int leftMovement;
    public TileRecode tileRecode = new TileRecode();
}


[Serializable]
public enum Phase
{
    none,
    Drop,
    Focus,
    FocusWait,
    Calculate,
    Blast,
    DestroyerWait,
    MoveTest,
}


public class TileRecode
{
    private List<int> tileRecord = new List<int>();

    public TileRecode()
    {
        tileRecord.Clear();

        for (int i = 0; i < System.Enum.GetValues(typeof(TileType)).Length; i++)
        {
            tileRecord.Add(0);
        }
    }

    public void Record(TileType type, int count = 1)
    {
        int index = (int)type;
        tileRecord[index] += count;
    }

    public List<int> GetRecord()
    {
        return tileRecord;
    }

    public int GetRecord(TileType type)
    {
        int index = (int)type;
        return tileRecord[index];
    }

    public void Clear()
    {
        for (int i = 0; i < tileRecord.Count; i++)
        {
            tileRecord[i] = 0;
        }
    }
    public int getSize()
    {
        return System.Enum.GetValues(typeof(TileType)).Length;
    }
}

public class SimpleEvent<T1, T2, T3>
{
    private readonly List<Action<T1, T2, T3>> actionList = new List<Action<T1, T2, T3>>();

    public void AddListener(Action<T1, T2, T3> action)
    {
        actionList.Add(action);
    }

    public void RemoveListener(Action<T1, T2, T3> action)
    {
        actionList.Remove(action);
    }

    public void Invoke(T1 arg1, T2 arg2, T3 arg3)
    {
        foreach (var action in actionList)
        {
            action?.Invoke(arg1, arg2, arg3);
        }
    }
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

