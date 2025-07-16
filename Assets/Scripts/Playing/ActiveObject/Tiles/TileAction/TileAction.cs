using UnityEngine;

public abstract class TileAction : MonoBehaviour
{
    protected Tile tile;
    public void Init(Tile tile)
    {
        this.tile = tile;

    }
    public virtual void Invoke()
    {

        if (tile == null)
        {
            tile = GetComponent<Tile>();

            if (tile == null)
            {
                Debug.LogError($"{gameObject.name}에게 Tile이 없습니다!");
                return;
            }

            Debug.LogWarning($"{gameObject.name}에 있는 {GetType()}가 Init되지 않았습니다. 자동으로 Init합니다.");
        }

        if (!tile.isActive)
        {
            Debug.LogWarning($"{gameObject.name}에 있는{GetType()}가 비활성 상태이므로 Invoke를 무시합니다.");
            return;
        }

        OnInvoke();
    }

    virtual protected void OnInvoke()
    {
        Debug.LogWarning($"{gameObject.name}에 있는 {GetType()}가 Invoke되었지만 구성되지 않아 무시되었습니다.");
    }


        public T GetTileFromWorld<T>(Vector2 direction, bool isGlobal = false)
    {
        return GetTileFromWorld<T>(direction, 1, isGlobal);
    }

    public T GetTileFromWorld<T>(Vector2 direction, int revisionMultiple, bool isGlobal = false)
    {
        int layerMask = isGlobal ? ~0 : 1 << gameObject.layer;
        // 콜라이더 바깥으로 GetTileFromWorld 시작 지점 계산
        Vector2 start = (Vector2)transform.position + direction * Utils.RAYCASY_REVISION * revisionMultiple;

        // GetTileFromWorld 수행
#if UNITY_EDITOR
        DrawOverlapBox(start, Utils.FloatToVector2(Utils.TILE_SIZE), transform.rotation.z, Color.red);
#endif
        Collider2D hit = Physics2D.OverlapBox(start, Utils.FloatToVector2(Utils.TILE_SIZE), transform.rotation.z, layerMask);

        // Debug.Log(hit.collider.name
        return hit ? hit.GetComponent<T>() : default;
    }
    
    
    
#if UNITY_EDITOR
    void DrawOverlapBox(Vector2 center, Vector2 size, float angleDeg, Color color, float duration = 0.05f)
    {
        float angleRad = angleDeg * Mathf.Deg2Rad;
        Vector2 right = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        Vector2 up = new Vector2(-right.y, right.x);

        Vector2 extentsX = right * (size.x / 2f);
        Vector2 extentsY = up * (size.y / 2f);

        Vector2 topRight = center + extentsX + extentsY;
        Vector2 topLeft = center - extentsX + extentsY;
        Vector2 bottomLeft = center - extentsX - extentsY;
        Vector2 bottomRight = center + extentsX - extentsY;

        Debug.DrawLine(topLeft, topRight, color, duration);
        Debug.DrawLine(topRight, bottomRight, color, duration);
        Debug.DrawLine(bottomRight, bottomLeft, color, duration);
        Debug.DrawLine(bottomLeft, topLeft, color, duration);
    }
#endif
}
