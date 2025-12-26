using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Managers<InputManager>
{
    private bool NeedMoveTest;


    public void MoveTest()
    {
        NeedMoveTest = true;
    }

    private void Update()
    {
        if (EventManager.Instance.phase == Phase.Focus && !NeedMoveTest && Utils.IsDown(out Vector2 pos))
        {
            if (Camera.main == null) return;
            
            Collider2D hit = Physics2D.OverlapBox(pos, Utils.FloatToVector2(Utils.MOUSE_SIZE), transform.rotation.z, 64);
#if UNITY_EDITOR
            DrawOverlapBox(pos, Utils.FloatToVector2(Utils.MOUSE_SIZE), 0, Color.red);
#endif
            // Debug.Log("Mouse Clicked at: " + pos + " Hit: " + hit);

            if(hit == null || hit.GetComponent<Tile>() == null) return;

            
            hit.GetComponent<Tile>().Focus();

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
