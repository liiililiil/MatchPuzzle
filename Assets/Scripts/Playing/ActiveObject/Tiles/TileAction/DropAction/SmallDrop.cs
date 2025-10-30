using System.Collections;
using UnityEngine;

// 1X1 타일 드랍 액션
public class SmallDrop : DropAction, IDropAction
{
    private Coroutine coroutine;

    // 드랍 가능
    public bool isCanDrop { get { return true; } }
    protected override void OnInvoke()
    {

        if (coroutine != null)
        {
            Debug.LogWarning("코루틴이 이미 실행 중이라 무시되었습니다.");
            return;
        }

        // 밑으로 하강 연산
        if (Drop(GetTileFromWorld<IDropAction>(-transform.up), -transform.up)) return;

        //옆으로 하강 연산
        foreach (Vector2 dir in Utils.xDirections)
        {
            //상대 좌표를 절대 좌표로 변환
            Vector2 worldDir = dir == Vector2.right ? transform.right : -transform.right;

            //밑에 타일이 있는지 검사
            IDropAction belowTile = GetTileFromWorld<IDropAction>((Vector2)(-transform.up) + worldDir);
            if (belowTile == null)
            {
                
                //옆에 떨어질 타일이 있는지 검사
                IDropAction sideTile = GetTileFromWorld<IDropAction>(worldDir);

                if (sideTile != null && sideTile.isCanDrop)
                    continue;

                //두칸 옆에 떨어질 타일이 있는지 검사
                if (dir == Vector2.right)
                {
                    sideTile = GetTileFromWorld<IDropAction>(worldDir, 2);
                    if (sideTile != null && sideTile.isCanDrop)
                        continue;
                }

                //전부 없으면 하강
                if (Drop(belowTile, (Vector2)(-transform.up) + worldDir)) return;
            }
        }

        //이전에 떨어진적 있다면 감소
        if (isDrop)
        {
            isDrop = false;
            EventManager.Instance.movingTiles--;
            tile.Calculate();
        }
    }

    //하강
    protected bool Drop(IDropAction belowTile, Vector2 dir)
    {
        if (belowTile == null)
        {
            //이동
            if (!isDrop)
            {
                isDrop = true;
                EventManager.Instance.movingTiles++;
            }

            Vector2 startPos = transform.position;

            //이동 위치
            Vector2 targetPos = startPos + dir * Utils.TILE_GAP;

            //위 하강 예약
            Tile[] tiles = new Tile[] { GetTileFromWorld<Tile>(transform.up), GetTileFromWorld<Tile>(transform.up + transform.right), GetTileFromWorld<Tile>(transform.up - transform.right) };

            //히트박스는 미리 이동해놓기 
            tile.rigidbody2D.MovePosition(targetPos);

            coroutine = StartCoroutine(moveMent2D(startPos, targetPos));

            // 하강
            foreach (Tile tile in tiles) if (tile != null) tile.Drop();

            return true;

        }

        return false;
    }

    //2D 보간 이동 코루틴
    IEnumerator moveMent2D(Vector2 startPos, Vector2 targetPos)
    {
        EventManager.Instance.dropTiles++;

        float time = 0f;
        while (time <= 1f)
        {
            tile.sprite.transform.position = Vector2.Lerp(startPos, targetPos, time);
            time += Time.deltaTime * Utils.MOVEMENT_SPEED;
            yield return null;
        }

        // 위치 보정
        tile.sprite.transform.position = targetPos;
        tile.sprite.transform.localPosition = Vector3.zero;

        EventManager.Instance.dropTiles--;

        //하강 완료
        coroutineEnd();
    }

    //코루틴 종료 처리
    protected void coroutineEnd()
    {
        StopCoroutine(coroutine);
        coroutine = null;

        tile.Drop();
    }
}
