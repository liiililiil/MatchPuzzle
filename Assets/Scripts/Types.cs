using System;
using System.Runtime.CompilerServices;

[Serializable]
public enum tileType : byte{
    Blast,
    Block,
    Blue,
    Green,
    Purple,
    Red,
    Box,
    XBomb,
    YBomb,
    BigBomb,
    ColorBomb
}

public struct Chain{
    private ushort _self;
    private ushort _total;
    public ushort self
{
        get => _self;
        set => _self = value;
    }
    public ushort total{
        get => _total;
        set => _total = value;
    }

    public Chain(ushort self, ushort total){
        _self = self;
        _total = total;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Reset(){
        self = 0; 
        total = 0;
    }

}

[Serializable]
public struct Vector2Byte{
    private byte _x;
    private byte _y;

    public byte x{
        get => _x;
        set => _x = value;
    }

    public byte y{
        get => _y;
        set => _y = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Vector2Byte(byte x, byte y){
        _x = x;
        _y = y;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]

    public Vector2Byte(short x, short y){
        if(x > 255 || y > 255 || x < 0 || y < 0) throw new ArgumentOutOfRangeException($"x or y is out of range 0~255 (x:{x}, y:{y})");
        _x = (byte)x;
        _y = (byte)y;
    }
    
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]

    public Vector2Byte Add(short x, short y){
        return new Vector2Byte((short)(_x+x),(short)(_y+y));
    }


    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(byte x, byte y){
        return _x == x && _y == y;
    }



    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2Byte operator +(Vector2Byte a, Vector2Byte b)
    {
        return new Vector2Byte((byte)(a.x + b.x), (byte)(a.y + b.y));
    }

}