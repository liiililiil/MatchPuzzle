using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class MoveableFocus : TileFocusAction, ITileFocusAction
{
    private Coroutine focusCoroutine, moveCoroutine;
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
        Tile[,] tilesRecord = new Tile[2, 2];

        //움직일 수 있는 타일 조사
        foreach (Vector2Int dir in Utils.directions)
        {
            Tile tile = Utils.TryGetTile(transform.position, transform.TransformDirection((Vector2)dir), Utils.TILE_GAP);
            bool result;

            // Debug.Log(tile);

            if (tile != null && tile.GetComponent<ITileFocusAction>().isCanFocus)
            {
                result = true;
                SetVector2Array(ref tilesRecord, dir, tile);
            }
            else
            {
                result = false;
            }

            SetVector2Array(ref isCanMove, dir, result);

        }

        Vector2Int posRecord = Vector2Int.zero;

        //포커싱 기록 시작
        while (InputManager.Instance.isTouch)
        {
            
            Vector2 dis = (Vector2)((Vector3)InputManager.Instance.touchPos - transform.position);

            Vector2Int focusAt;

            // 일정 범위 내에서는 0,0 처리
            if (Mathf.Abs(dis.x) <= 0.3f && Mathf.Abs(dis.y) <= 0.3f)
            {
                focusAt = Vector2Int.zero;
            }
            else
            {
                // X와 Y 중 큰 절대값 방향을 우선
                if (Mathf.Abs(dis.x) > Mathf.Abs(dis.y))
                {
                    // X축 우세: 왼쪽(-1) 또는 오른쪽(1)
                    focusAt = new Vector2Int(dis.x < 0 ? -1 : 1, 0);
                }
                else
                {
                    // Y축 우세: 아래(-1) 또는 위(1)
                    focusAt = new Vector2Int(0, dis.y < 0 ? -1 : 1);
                }
            }

            if (focusAt != Vector2Int.zero && GetVector2Array(ref isCanMove, focusAt) && focusAt != posRecord)
            {
                if(posRecord != Vector2Int.zero)
                    GetVector2Array(ref tilesRecord, posRecord).GetComponent<ITileFocusAction>().UnFocus();
                posRecord = focusAt;
                OnFocus(focusAt);
                GetVector2Array(ref tilesRecord, focusAt).GetComponent<ITileFocusAction>().OnFocus(-focusAt);

            }
            else if (focusAt == Vector2Int.zero)
            {
                if(posRecord != Vector2Int.zero)
                    GetVector2Array(ref tilesRecord, posRecord).GetComponent<ITileFocusAction>().UnFocus();
                posRecord = Vector2Int.zero;

                tile.GetComponent<ITileFocusAction>().UnFocus();
            }


            yield return null;
        }

        // Debug.Log($"포커스 해제 {posRecord}" );

        //포커싱 실행
        //움직였으면 이동
        if (posRecord != Vector2Int.zero)
        {
            //본인 이동
            Move(posRecord);
            EventManager.Instance.InvokeReMove.Add(() => Move(-posRecord));

            //움직이기 전에 어떤 타일이랑 교체됬는지 기록
            tile.switchedTileType = GetVector2Array(ref tilesRecord, posRecord).tileType;
            Tile tileRecord = GetVector2Array(ref tilesRecord, posRecord);

            //기록된 타일도 동일하게 기록
            tileRecord.switchedTileType = tile.tileType;
            
            //상대 이동
            tileRecord.GetComponent<ITileFocusAction>().Move(-posRecord);
            EventManager.Instance.InvokeReMove.Add(() => tileRecord.GetComponent<ITileFocusAction>().Move(posRecord));

            // 둘다 폭탄이거나 이 타일이 컬러 폭탄이면 그낭 소멸
            if
            (
                (TILE_CONSTANT.BOMB_TILES.Contains(tile.tileType) && TILE_CONSTANT.BOMB_TILES.Contains(tileRecord.tileType) && tileRecord.tileType != TileType.ColorBomb) ||
                (tile.tileType == TileType.ColorBomb)
            )
            {
                EventManager.Instance.InvokeFocusBlast.Add(() => tileRecord.GetComponent<Tile>().Disable(true));
            }
            else
            {
                tileRecord.Calculate();
            }
            

            if
            (
                (tile.tileType != TileType.ColorBomb && tileRecord.tileType == TileType.ColorBomb)

            )
            {
                EventManager.Instance.InvokeFocusBlast.Add(() => tile.Disable(true));
            }
            else
            {
                tile.Calculate();
            }
        }

    }

    private void SetVector2Array<T>(ref T[,] array, Vector2Int dir, T value)
    {
        int x, y;

        if (dir.x == 1) { x = 1; y = 0; }
        else if (dir.x == -1) { x = 0; y = 0; }
        else if (dir.y == 1) { x = 0; y = 1; }
        else if (dir.y == -1) { x = 1; y = 1; }
        else { throw new ArgumentException("Invalid direction: " + dir); }

        array[x, y] = value;
    }
 
    private T GetVector2Array<T>(ref T[,] array, Vector2Int dir) 
    { 
        int x, y;

        if (dir.x == 1) { x = 1; y = 0; }
        else if (dir.x == -1) { x = 0; y = 0; }
        else if (dir.y == 1) { x = 0; y = 1; }
        else if (dir.y == -1) { x = 1; y = 1; }
        else { throw new ArgumentException("Invalid direction: " + dir); }

        return array[x, y];

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
        if (moveCoroutine != null)
        {
            return;
            // StopCoroutine(focusCoroutine);
            // EventManager.Instance.moveTestTiles--;
            // focusCoroutine = null;
        }
        EventManager.Instance.moveTestTiles++;

        Vector2 start = transform.position;
        this.moveAt = moveTo;

        Vector2 end = (Vector2)transform.position + (moveAt * Utils.FloatToVector2(Utils.TILE_GAP));

        tile.rigidbody2D.MovePosition(end);

        moveCoroutine = StartCoroutine(MoveTo(start, end));
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
        CourtineStop(ref moveCoroutine);
    }

    private void CourtineStop(ref Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
 

}