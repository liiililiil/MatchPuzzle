public class Chain{
    private ushort _self;
    private ushort _total;
    public ushort self{
        get => _self;
        set => _self = value;
    }

    public ushort total{
        get => _total;
        set => _total = value;
    }

    public void Reset(){
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
public enum TileType: ushort{
    Red = 0,
    Green = 1,
    Blue = 2,
    Purple = 3,
    BigBomb = 4,
    XBomb = 5,
    YBomb = 6,
    ColorBomb = 7,
    box = 8
    
}