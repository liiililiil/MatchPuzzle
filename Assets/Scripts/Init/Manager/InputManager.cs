using UnityEngine;

public class InputManager : Managers<InputManager>
{
    private bool NeedMoveTest;

    private Touch touch;
    public bool isTouch {get; private set;}
    public Vector2 touchPos {get; private set;}


    public void MoveTest()
    {
        NeedMoveTest = true;
    }

    private void Update()
    {
        TouchDetect();

        if (EventManager.Instance.phase == Phase.Focus && !NeedMoveTest &&isTouch)
        {
            if (Camera.main == null) return;
            
            Collider2D hit = Physics2D.OverlapBox(touchPos, Utils.FloatToVector2(Utils.MOUSE_SIZE), transform.rotation.z, 64);
#if UNITY_EDITOR
            DrawOverlapBox(touchPos, Utils.FloatToVector2(Utils.MOUSE_SIZE), 0, Color.red);
#endif
            // Debug.Log("Mouse Clicked at: " + pos + " Hit: " + hit);

            if(hit == null || hit.GetComponent<Tile>() == null) return;

            
            hit.GetComponent<Tile>().Focus();

        }
    }

    private void TouchDetect()
    {
        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
        }
        else
        {
            return;
        }

        if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {

            // Debug.Log("Touch End");
            isTouch = false;
            touchPos = Vector2.zero;
        }
        else
        {
            // Debug.Log($"Touching {touch.phase}" );
            isTouch = true;
            touchPos = Camera.main.ScreenToWorldPoint(touch.position);
        }

    }
    public static bool IsDown()
    {

        if (Utils.IS_MOBLIE)
        {
            if(Input.touchCount == 0)
            {
                return false;
            }

            Touch touch = Input.GetTouch(Input.touchCount - 1);

            if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                return false;
            }

            return true;
        }
        else
        {
            if(!UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
            {
                return false;
            }

            return Input.GetMouseButtonDown(0);
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
