using UnityEngine;
using System.Runtime.CompilerServices;
public static class Utils
{
    public const float TILE_SIZE = 0.9f;
    public const float TILE_GAP = 1f;
    public const float RAYCASY_LENGHT = 0.2f;
    public const float RAYCASY_REVISION = 1f;
    public const float WAIT_POS_X = 100f;
    public const float WAIT_Pos_Y = 100f;
    public const float ACCELERATION_SPEED = 50f;
    public const float MOVEMENT_SPEED = 20f;
    public const int TILETYPE_LENGHT = 9;
    public const int EFFECTTYPE_LENGHT = 1;
    public const float EFFECT_SPEED = 20f;

    public const int MOVEMENT_DURATION = 100; // 이동 지속 시간 (ms)

    // public const string StagePath = @"\Stage\"



    //반복용
    public static readonly Vector2[] xDirections = { Vector2.left, Vector2.right };
    public static readonly Vector2[] yDirections = { Vector2.up, Vector2.down };
    public static readonly Vector2[] directions = { Vector2.left, Vector2.right, Vector2.up, Vector2.down };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 FloatToVector2(float value)
    {
        return new Vector2(value, value);
    }
    

    

}
