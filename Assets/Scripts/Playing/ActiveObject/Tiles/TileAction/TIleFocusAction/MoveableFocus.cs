using System.Collections;
using UnityEngine;

public class MoveableFocus : TileFocusAction, ITileFocusAction
{
    private Coroutine focusCoroutine;
    private TileType switchingTo;
    public bool isCanFocus { get => true; }

    Vector2 moveAt;

    protected override void OnInvoke()
    {
        StartCoroutine(whileFocus());

    }

    IEnumerator whileFocus()
    {

        bool[,] isCanMove = new bool[2, 2];
        //움직일 수 있는 타일 조사
        foreach (Vector2Int dir in Utils.directions)
        {
            Tile tile = GetTileFromWorld<Tile>(dir);
            bool result;

            if (tile != null && tile.GetComponent<ITileFocusAction>().isCanFocus)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            SetIsCanMove(ref isCanMove, dir, result);

        }

        Tile tileRecord = null;
        Vector2Int posRecord = Vector2Int.zero;

        //포커싱 기록 시작
        while (Input.GetMouseButton(0))
        {
            Vector2 dis = (Vector2)Input.mousePosition - (Vector2)Camera.main.WorldToScreenPoint(transform.position);

            Vector2Int focusAt;
            // Debug.Log(dis);
            if (Mathf.Abs(dis.x) <= 30f && Mathf.Abs(dis.y) <= 30f) focusAt = Vector2Int.zero;
            else if (Mathf.Abs(dis.x) > Mathf.Abs(dis.y))
            {
                if (dis.x < 0)
                    focusAt = new Vector2Int(-1, 0);
                else
                    focusAt = new Vector2Int(1, 0);

            }
            else
            {
                if (dis.y < 0)
                    focusAt = new Vector2Int(0, -1);

                else
                    focusAt = new Vector2Int(0, 1);

            }

            if (focusAt != posRecord && focusAt != Vector2Int.zero)
            {
                if (GetIsCanMove(focusAt, isCanMove))
                {
                    if (tileRecord != null)
                        tileRecord.GetComponent<ITileFocusAction>().UnFocus();

                    tileRecord = GetTileFromWorld<Tile>(focusAt);
                    posRecord = focusAt;
                    OnFocus(focusAt);
                    tileRecord.GetComponent<ITileFocusAction>().OnFocus(-focusAt);
                    // Debug.Log($"{focusAt}, {tileRecord.name}", this);

                }
            }
            else if (focusAt == Vector2Int.zero)
            {
                if (tileRecord != null)
                {
                    tileRecord.GetComponent<ITileFocusAction>().UnFocus();
                    tileRecord = null;
                    posRecord = Vector2Int.zero;
                }

                tile.GetComponent<ITileFocusAction>().UnFocus();
            }


            yield return null;
        }

        //포커싱 실행
        //움직였으면 이동
        if (posRecord != Vector2Int.zero)
        {
            //본인 이동
            Move(posRecord);
            EventManager.Instance.InvokeReMove.Add(() => Move(-posRecord));
            //혹시 모를 Null 감지
            if (tileRecord != null)
            {
                //움직이기 전에 어떤 타일이랑 교체됬는지 기록
                tile.switchedTileType = tileRecord.tileType;

                //상대 이동
                tileRecord.GetComponent<ITileFocusAction>().Move(-posRecord);
                EventManager.Instance.InvokeReMove.Add(() => tileRecord.GetComponent<ITileFocusAction>().Move(posRecord));
                EventManager.Instance.InvokeFocusBlast.Add(() => tileRecord.GetComponent<Tile>().Disable(true));


                tileRecord.Calculate();
            }
            else
            {
                Debug.LogError("MoveableFocus: 타일이 없는데 움직임!", this);

            }

            tile.Calculate();
        }

        

        EventManager.Instance.MoveTest();
    }

    private void SetIsCanMove(ref bool[,] isCanMove, Vector2Int dir, bool value)
    {
        if (dir.x == -1 && dir.y == 0)
        {
            isCanMove[0, 0] = value;
        }
        else if (dir.x == 1 && dir.y == 0)
        {
            isCanMove[1, 0] = value;
        }
        else if (dir.x == 0 && dir.y == -1)
        {
            isCanMove[0, 1] = value;
        } 
        else if (dir.x == 0 && dir.y == 1) 
        { 
            isCanMove[1, 1] = value; 
        } 
        else 
        { 
            throw new System.ArgumentException("Invalid direction: " + dir); 
        } 
    } 
 
    private bool GetIsCanMove(Vector2Int dir, bool[,] isCanMove) 
    { 
        if (dir.x == -1 && dir.y == 0) 
        { 
            return isCanMove[0, 0]; 
        }
        else if (dir.x == 1 && dir.y == 0)
        {
            return isCanMove[1, 0];
        }
        else if (dir.x == 0 && dir.y == -1)
        {
            return isCanMove[0, 1];
        }
        else if (dir.x == 0 && dir.y == 1)
        {
            return isCanMove[1, 1];
        }
        else
        {
            throw new System.ArgumentException("Invalid direction: " + dir);
        }
    }
    public override void OnFocus(Vector2Int at)
    {
        // Debug.Log($"포커싱: {at}", this);

        if (focusCoroutine != null)
        {
            StopCoroutine(focusCoroutine);
            focusCoroutine = null;
        }

        focusCoroutine = StartCoroutine(Focus(at));
    }

    public override void UnFocus()
    {
        // Debug.Log("포커싱 해제", this);
        if (focusCoroutine != null)
        {
            StopCoroutine(focusCoroutine);
            focusCoroutine = null;
        }

        focusCoroutine = StartCoroutine(Focus(Vector2Int.zero));
    }

    IEnumerator Focus(Vector2Int at)
    {
        Vector2 start = tile.sprite.transform.position;
        Vector2 end = (Vector2)transform.position + (at * Utils.FloatToVector2(Utils.FOCUS_ANIMATION_MOVING_LENGHT));

        float time = 0f;
        while (time < 1)
        {
            time += Time.deltaTime * GameSpeedManager.FOCUS_ANIMATION_SPEED;
            tile.sprite.transform.position = Vector2.Lerp(start, end, time);
            yield return null;
        }

        tile.sprite.transform.position = end;
    }
    public override void Move(Vector2Int moveTo, bool isOperand = false)
    {
        tile.switched = true;
        if (focusCoroutine != null)
        {
            StopCoroutine(focusCoroutine);
            focusCoroutine = null;
        }
        EventManager.Instance.moveTestTiles++;

        Vector2 start = transform.position;
        this.moveAt = moveTo;

        Vector2 end = (Vector2)transform.position + (moveAt * Utils.FloatToVector2(Utils.TILE_GAP));

        tile.rigidbody2D.MovePosition(end);

        focusCoroutine = StartCoroutine(MoveTo(start, end));
    }

    private IEnumerator MoveTo(Vector2 start, Vector2 end)
    {
        float time = 0f;
        while (time < 1)
        {
            time += Time.deltaTime * GameSpeedManager.FOCUS_MOVE_SPEED;
            tile.sprite.transform.position = Vector2.Lerp(start, end, time);
            yield return null;
        }

        tile.sprite.transform.position = end;

        tile.Calculate();
        EventManager.Instance.moveTestTiles--;
    }
 

}