using UnityEngine;
using System.Runtime.CompilerServices;
using System;
public static class Utils
{
    // 전체 속도에 대한 배율
    public const float SPEED_MULTIPLY = 3f;


    public const float TILE_SIZE = 0.9f;
    public const float TILE_GAP = 1f;
    public const float RAYCASY_LENGHT = 0.2f;
    public const float RAYCASY_REVISION = 1f;
    public const float WAIT_POS_X = 100f;
    public const float WAIT_Pos_Y = 100f;
    public const float MOVEMENT_SPEED = 20f * SPEED_MULTIPLY;
    public const int TILETYPE_LENGHT = 9;
    public const int EFFECTTYPE_LENGHT = 1;
    public const float EFFECT_DURATION = 15f * SPEED_MULTIPLY;
    public const float DESTROYER_FORWARD_SPEED = 20f * SPEED_MULTIPLY;
    public const float EXTINCTION_DURATION = 1.5f;
    public const float MOUSE_SIZE = 0.2f;
    public const float DESTROYER_TRAIL_EFFECT_SPAWN_SPEED = 0.5f * SPEED_MULTIPLY;
    public const float FOCUS_ANIMATION_MOVING_LENGHT = 0.1f;
    public const float FOCUS_ANIMATION_SPEED = 10f * SPEED_MULTIPLY;
    public const float FOCUS_MOVE_SPEED = 10f * SPEED_MULTIPLY;
    public const float SHOOTER_SCALE_SPEED = 1f * SPEED_MULTIPLY;
    public const float SHOOTER_CHILD_MOVE_SPEED = 1f * SPEED_MULTIPLY;

    // public const int MOVEMENT_DURATION = 100; // 이동 지속 시간 (ms)

    // public const string StagePath = @"\Stage\"



    //반복용
    public static readonly Vector2Int[] xDirections = { Vector2Int.left, Vector2Int.right };
    public static readonly Vector2Int[] yDirections = { Vector2Int.up, Vector2Int.down };
    public static readonly Vector2Int[] directions = { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 FloatToVector2(float value)
    {
        return new Vector2(value, value);
    }

    internal static int GetEnumLength<T>()
    {
        throw new NotImplementedException();
    }


}
