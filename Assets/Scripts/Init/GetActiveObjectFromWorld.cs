using UnityEngine;

public class GetActiveObjectFromWorld : MonoBehaviour
{
    protected T GetTileFromWorld<T>(Vector2 direction, int revisionMultiple = 1)
    {
        int layerMask = 1 << gameObject.layer;
        // 위치 계산
        Vector2 target = (Vector2)transform.position + direction * Utils.RAYCASY_REVISION * revisionMultiple;

        // GetTileFromWorld 수행
#if UNITY_EDITOR
        DrawOverlapBox(target, Utils.FloatToVector2(Utils.TILE_SIZE), transform.rotation.z, Color.red);
#endif
        Collider2D hit = Physics2D.OverlapBox(target, Utils.FloatToVector2(Utils.TILE_SIZE), transform.rotation.z, layerMask);

        if (hit == null)
        {
            // Debug.Log($"{target}에서 {layerMask} 레이어에 공간에 Null이 발생했습니다!", this);

            hit = Physics2D.OverlapBox(target, Utils.FloatToVector2(Utils.TILE_SIZE), transform.rotation.z, layerMask);



        }
        else
        {
            // Debug.Log($"출동한 객체 : {hit.gameObject.name}", this);
        }

        // Debug.Log(hit.collider.name
        return hit ? hit.GetComponent<T>() : default;
    }
    
    
    
#if UNITY_EDITOR
    protected void DrawOverlapBox(Vector2 center, Vector2 size, float angleDeg, Color color, float duration = 0.05f)
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
