public struct Chain{
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

    public static Chain operator +(Chain a, Chain b)
    {
        return new Chain
        {
            self = (ushort)(a.self + b.self),
            total = (ushort)(a.total + b.total)
        };
    }
}