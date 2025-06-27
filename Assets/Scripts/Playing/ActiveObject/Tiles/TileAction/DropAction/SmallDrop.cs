using System.Collections;
using UnityEngine;

public class SmallDrop : TileAction, IDropAction
{
    private Coroutine coroutine;
    private bool isDrop = false;

    public bool isCanDrop { get { return true; } }
    protected override void OnInvoke()
    {
        if (coroutine != null)
        {
            Debug.LogWarning("코루틴이 이미 실행 중이라 무시되었습니다.");
            return;
        }

        // 밑으로 하강 연산
        if (DropCheck(GetTileFromWorld<IDropAction>(-transform.up, true), -transform.up)) return;

        //옆으로 하강 연산
        foreach (Vector2 dir in Utils.xDirections)
        {
            Vector2 worldDir = dir == Vector2.right ? transform.right : -transform.right;

            IDropAction belowTile = GetTileFromWorld<IDropAction>((Vector2)(-transform.up) +worldDir, true);
            if (belowTile == null)
            {
                //옆으로 떨어질 타일이 있는지 검사
                IDropAction sideTile = GetTileFromWorld<IDropAction>(worldDir, true);
                if (sideTile != null && sideTile.isCanDrop)
                    continue;

                //오른쪽으로 떨어질 타일이 있는지 검사
                if (dir == Vector2.right)
                {
                    sideTile = GetTileFromWorld<IDropAction>(worldDir, 2, true);
                    if (sideTile != null && sideTile.isCanDrop)
                        continue;
                }

                //전부 없으면 하강
                if (DropCheck(belowTile, (Vector2)(-transform.up) +worldDir)) return;
            }
        }

        //떨어지지않음

        //이전에 떨어진적 있다면 감소
        if (isDrop)
        {
            isDrop = false;
            EventManager.Instance.movingTiles--;
            tile.Calculate();
        }
    }


    protected bool DropCheck(IDropAction belowTile, Vector2 dir)
    {
        // Debug.Log(belowTile);
        if (belowTile == null)
        {
            //이동
            if (!isDrop)
            {
                isDrop = true;
                EventManager.Instance.movingTiles++;
            }

            // if (coroutine != null)
            // {
            //     Debug.Log("중복 코루틴 실행 감지됨!");
            //     return true;
            // }

            coroutine = StartCoroutine(moveMent2D(dir));

            return true;

        }

        return false;
    }


    IEnumerator moveMent2D(Vector2 direction)
    {
        //일단 위에 타일 구하기
        Tile[] aboveTile = { GetTileFromWorld<Tile>(transform.up, true), GetTileFromWorld<Tile>(transform.up + transform.right, true), GetTileFromWorld<Tile>(transform.up - transform.right, true) };

        // 현재 위치를 그리드에 정렬
        Vector2 startPos = new Vector2(
            Mathf.Round(transform.position.x / Utils.TILE_GAP) * Utils.TILE_GAP,
            Mathf.Round(transform.position.y / Utils.TILE_GAP) * Utils.TILE_GAP
        );

        //이동 위치
        Vector2 targetPos = startPos + direction * Utils.TILE_GAP;

        //히트박스는 미리 이동해놓기 
        transform.position = targetPos;

        float time = 0f;
        while (time <= 1f)
        {
            tile.sprite.transform.position = Vector2.Lerp(startPos, targetPos, time);
            time += Time.deltaTime * Utils.MOVEMENT_SPEED;
            yield return null;
        }

        // 위치 정밀 보정
        tile.sprite.transform.position = targetPos;

        //위 하강 등록
        foreach (Tile tile in aboveTile) if (tile != null) tile.Drop();

        //하강 완료
        coroutineEnd();
    }

    protected void coroutineEnd()
    {
        StopCoroutine(coroutine);
        coroutine = null;

        tile.Drop();
    }
}
