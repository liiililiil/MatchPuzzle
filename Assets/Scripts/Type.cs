
public class Chain
{
    private ushort _self;
    private ushort _total;
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

[System.Serializable]
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
[System.Serializable]
public class TileData
{
    public UnityEngine.GameObject prefab;
    public TileType tileType;
    public System.Collections.Generic.Queue<UnityEngine.GameObject> pooling;

}

