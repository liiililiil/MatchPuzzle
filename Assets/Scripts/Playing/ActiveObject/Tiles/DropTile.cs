using System;
using System.Collections;
using UnityEngine;

public abstract class DropTile : Tile, ITile
{
    public sealed override bool isCanDrop { get => true; }


    public override sealed void Drop()
    {
        //이미 이동중인데 함수 호출되면 반환
        // if (isDrop) return;

        //코루틴 실행중이라면 반환 
        if (isCoroutineRunning()) return;

        if (boxCollider2D == null) boxCollider2D = this.GetComponent<BoxCollider2D>();

        // isDrop = false;
        EventManager.Instance.OnDrop += DropCalculation;
    }

    protected void DropCalculation()
    {
        //밑으로 하강 연산
        ITile belowTile = GetTileFromWorld(Vector2.down, true);
        if (DropTest(belowTile, -transform.up)) return;

        //옆으로 하강 연산
        foreach (Vector2 dir in Utils.xDirections)
        {
            belowTile = GetTileFromWorld(Vector2.down + dir, true);
            if (belowTile == null)
            {
                //옆으로 하강하기전에 떨어질 타일이 있으면 떨어지지 않음
                ITile sideTile = GetTileFromWorld(dir, true);
                if (sideTile != null && sideTile.isCanDrop)
                {
                    continue;
                }

                //오른쪽으로 떨어질 타일이 있으면 떨어지지 않음
                //
                if (dir == Vector2.right)
                {
                    sideTile = GetTileFromWorld(dir, 2, true);
                    if (sideTile != null && sideTile.isCanDrop)
                        continue;

                }

                //떨어트리기
                if (DropTest(belowTile, -transform.up + (dir == Vector2.right ? transform.right : -transform.right))) return;
            }
        }

        if (isDrop)
        {
            isDrop = false;
            EventManager.Instance.movingTiles--;
            EventManager.Instance.OnCalculate += Calculate;

            Debug.Log("- 됨");
        }
    }

    protected bool DropTest(ITile belowTile, Vector3 dir)
    {
        if (belowTile == null /*|| belowTile.isDrop*/)
        {
            //이동
            if (!isDrop)
            {
                isDrop = true;
                EventManager.Instance.movingTiles++;
                Debug.Log("+ 됨");
            }

            coroutine = StartCoroutine(moveMent2D(dir));

            return true;

        }

        return false;
    }

    IEnumerator moveMent2D(Vector2 direction)
    {
        //일단 위에 타일 구하기
        ITile[] aboveTile = { GetTileFromWorld(Vector2.up, true), GetTileFromWorld(Vector2.up + Vector2.right, true), GetTileFromWorld(Vector2.up + Vector2.left, true) };

        // 현재 위치를 그리드에 정렬
        Vector2 startPos = new Vector2(
            Mathf.Round(transform.position.x / Utils.TILE_GAP) * Utils.TILE_GAP,
            Mathf.Round(transform.position.y / Utils.TILE_GAP) * Utils.TILE_GAP
        );

        Vector2 targetPos = startPos + direction * Utils.TILE_GAP;

        transform.position = targetPos;

        float time = 0f;


        while (time <= 1f)
        {
            sprite.transform.position = Vector2.Lerp(startPos, targetPos, time);
            time += Time.deltaTime * Utils.MOVEMENT_SPEED;
            yield return null;
        }

        // 위치 정밀 보정
        sprite.transform.position = targetPos;

        //위 하강 등록
        foreach (ITile tile in aboveTile) if (tile != null /*&& !tile.isDrop*/) tile.Drop();
        coroutineEnd();

    }

    protected void coroutineEnd()
    {

        StopCoroutine(coroutine);
        coroutine = null;

        Drop();
    }

}

