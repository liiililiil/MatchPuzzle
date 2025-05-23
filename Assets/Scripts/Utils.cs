using UnityEngine;
using System.Runtime.CompilerServices;
public static class Utils
{
    public const float TILE_SIZE = 0.6f;
    public const float TILE_GAP = 0.6f;
    public const float RAYCASY_LENGHT = 0.2f;
    public const float RAYCASY_REVISION = 0.22f; 
    public const float WAIT_POS_X = 100f;
    public const float WAIT_Pos_Y = 100f;
    public const float MOVEMENT_SPEED = 2f;
    public const int TILETYPE_LENGHT = 8;



    //반복용
    public static readonly Vector2[] xDirections = { Vector2.left, Vector2.right };
    public static readonly Vector2[] yDirections = { Vector2.up, Vector2.down };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 FloatToVector2(float value)
    {
        return new Vector2(value, value);
    }

}
