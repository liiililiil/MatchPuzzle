using System.Collections;
using System.IO;
using UnityEngine;

public class SmallDrop : DropAction, IDropAction
{
    private Coroutine coroutine;

    public bool isCanDrop { get { return true; } }
    protected override void OnInvoke()
    {
        // Debug.Log(tile.isActive);
        // Debug.Log($"드랍 연산중 {gameObject.name}",gameObject);
        if (coroutine != null)
        {
            Debug.LogWarning("코루틴이 이미 실행 중이라 무시되었습니다.");
            return;
        }

        // 밑으로 하강 연산
        if (DropCheck(GetTileFromWorld<IDropAction>(-transform.up), -transform.up)) return;

        //옆으로 하강 연산
        foreach (Vector2 dir in Utils.xDirections)
        {
            Vector2 worldDir = dir == Vector2.right ? transform.right : -transform.right;

            IDropAction belowTile = GetTileFromWorld<IDropAction>((Vector2)(-transform.up) + worldDir);
            if (belowTile == null)
            {

                IDropAction sideTile = GetTileFromWorld<IDropAction>(worldDir);
                if (sideTile != null && sideTile.isCanDrop)
                    continue;

                //오른쪽으로 떨어질 타일이 있는지 검사
                if (dir == Vector2.right)
                {


                    sideTile = GetTileFromWorld<IDropAction>(worldDir, 2);
                    if (sideTile != null && sideTile.isCanDrop)
                        continue;
                }

                //전부 없으면 하강
                if (DropCheck(belowTile, (Vector2)(-transform.up) + worldDir)) return;
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
            // Debug.Log(belowTile,this);
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

            // 현재 위치를 그리드에 정렬
            // Vector2 startPos = new Vector2(
            //     Mathf.Round(transform.position.x / Utils.TILE_GAP) * Utils.TILE_GAP,
            //     Mathf.Round(transform.position.y / Utils.TILE_GAP) * Utils.TILE_GAP
            // );

            Vector2 startPos = transform.position;

            //이동 위치
            Vector2 targetPos = startPos + dir * Utils.TILE_GAP;

            //위 하강 예약
            Tile[] tiles = new Tile[] { GetTileFromWorld<Tile>(transform.up), GetTileFromWorld<Tile>(transform.up + transform.right), GetTileFromWorld<Tile>(transform.up - transform.right) };

            //히트박스는 미리 이동해놓기 
            tile.transform.position = targetPos;
            // tile.rigidbody2D.MovePosition(targetPos);

            coroutine = StartCoroutine(moveMent2D(startPos, targetPos));

            // 하강
            foreach (Tile tile in tiles) if (tile != null) tile.Drop();

            return true;

        }

        return false;
    }


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

        // 위치 정밀 보정
        tile.sprite.transform.position = targetPos;

        EventManager.Instance.dropTiles--;
        
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
