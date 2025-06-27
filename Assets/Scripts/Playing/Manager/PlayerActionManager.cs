using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapBox(pos, Utils.FloatToVector2(Utils.MOUSE_SIZE), transform.rotation.z, 1920);
            DrawOverlapBox(pos, Utils.FloatToVector2(Utils.MOUSE_SIZE), 0, Color.red);

            hit?.GetComponent<Tile>()?.OnplayerFocus();

        }
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
