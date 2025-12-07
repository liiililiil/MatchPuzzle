using UnityEngine;
using System.Runtime.CompilerServices;
using System;
public static class Utils
{


    public const float TILE_SIZE = 0.9f;
    public const float TILE_GAP = 1.5f;
    public const float RAYCASY_LENGHT = 0.2f;
    public const float WAIT_POS_X = 100f;
    public const float WAIT_POS_Y = 100f;
    public const int TILETYPE_LENGHT = 9;
    public const int EFFECTTYPE_LENGHT = 1;
    public const float EXTINCTION_DURATION = 1.5f;
    public const float MOUSE_SIZE = 0.2f;
    public const float FOCUS_ANIMATION_MOVING_LENGHT = 0.1f;

    public const float GAME_SPEED_MAX = 3f;




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

    //4방향으로 타일을 검사하여 정리 요청
    public static void CallOrganize(GameObject target)
    {
        //4 방향으로 Ray 쏘기
        foreach (Vector2 dir in Utils.directions)
        {
            // 상대 방향을 절대 방향으로 변환
            Vector2 worldDir = target.transform.TransformDirection(dir);

            // GetTileFromWorld를 이용해 타일을 찾고 정리 수행
            Tile tile = Utils.TryGetTile(target.transform.position, worldDir, 1);
            tile?.Organize();
        }
    }
    
    //모듈 탐색
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T TryGetAction<T>(Vector2 start, Vector2 direction, float distance, bool ignoreBlock = false) where T : class, ITileAction
    {
        return TryGetTile(start, direction, distance, ignoreBlock)?.GetComponent<T>();
    }

    //타일만 탐색
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Tile TryGetTile(Vector2 start, Vector2 direction, float distance, bool ignoreBlock = false){
        return TryGetGameObject(start, direction, distance, LayerMask.GetMask("Tile"), ignoreBlock)?.GetComponent<Tile>();
    }


    // 게임 오브젝트 탐색
    public static GameObject TryGetGameObject(Vector2 start, Vector2 direction, float distance, int layer, bool ignoreBlock = false)
    {
        Vector2 startPos = start + direction * ((Utils.TILE_SIZE/2) + 0.1f);
        float targetDistance = Mathf.Max(distance - ((Utils.TILE_SIZE/2) + 0.1f), 0.1f);
        
    #if UNITY_EDITOR
            DrawRayDebug(startPos, direction, targetDistance);
    #endif
        if (ignoreBlock)
        {
            layer |= LayerMask.GetMask("Block");
        }
            RaycastHit2D hit = Physics2D.Raycast(startPos, direction, targetDistance, layer);
            return hit.collider?.gameObject;
    }
        
        
        
    #if UNITY_EDITOR
        public static void DrawRayDebug(Vector2 center, Vector2 direction, float distance)
        {
            Debug.DrawRay(center, direction * distance, Color.red, 0.1f);
        }
    #endif
}

