using System;
using System.Collections;
using UnityEngine;

public abstract class DropTile : Tile, ITile
{
    public sealed override bool isCanDrop { get => true; }

    public override sealed void Drop()
    {
        //이미 이동중인데 함수 호출되면 반환
        if (isDrop) return;

        //코루틴 실행중이라면 반환 
        if (isCoroutineRunning()) return;

        if (boxCollider2D == null) boxCollider2D = this.GetComponent<BoxCollider2D>();

        isDrop = false;
        EventManager.Instance.OnDrop += DropCalculation;
    }

    protected void DropCalculation()
    {


        //밑으로 하강 연산
        ITile belowTile = Raycast(Vector2.down, 1, true);
        if (DropTest(belowTile, -transform.up)) return;

        //옆으로 하강 연산
        foreach (Vector2 dir in Utils.xDirections)
        {
            belowTile = Raycast(Vector2.down + dir, 1.7f, true);

            //옆으로 하강하기전에 떨어질 타일이 있으면 떨어지지않음
            ITile sideTile = Raycast(dir, 1, true);
            if (sideTile != null && sideTile.isCanDrop)
            {
                // sideTile.Drop();
                continue;
            }

            Vector2 worldDirection = transform.TransformDirection(dir);

            // 콜라이더 바깥으로 Raycast 시작 지점 계산
            Vector2 start = (Vector2)transform.position + worldDirection * (Utils.RAYCASY_REVISION * 3);

            // Raycast 수행
            Debug.DrawRay(start, worldDirection * Utils.RAYCASY_LENGHT, Color.blue, 0.01f);
            RaycastHit2D hit = Physics2D.Raycast(start, worldDirection, Utils.RAYCASY_LENGHT, -1);
            
            ITile tile = hit.collider ? hit.collider.GetComponent<ITile>() : null;



            if (sideTile != null && sideTile.isDrop)
            {
                continue;
            }

            if (DropTest(belowTile, -transform.up + (dir == Vector2.right ? transform.right : -transform.right))) return;
        }

        // // 이동했었고 이동이 끝났으면 위 타일에게 하강 호출
        // if (needCallDrop)
        // {
        //     ITile[] aboveTile = { Raycast(Vector2.up, 3, true), Raycast(Vector2.up + Vector2.right, 5f, true), Raycast(Vector2.up + Vector2.left, 5f, true) };
        //     foreach (ITile tile in aboveTile) if (tile != null && !tile.isDrop) tile.Drop();

        //     //본인은 계산대기
        //     EventManager.Instance.OnCalculate += Calculate;
        // }
    }

    protected bool DropTest(ITile belowTile, Vector3 dir)
    {
        if (belowTile == null || belowTile.isDrop)
        {
            //일단 위에 타일 구하기
            ITile[] aboveTile = { Raycast(Vector2.up, 3, true), Raycast(Vector2.up + Vector2.right, 5f, true), Raycast(Vector2.up + Vector2.left, 5f, true) };

            //이동
            isDrop = true;
            needCallDrop = true;

            EventManager.Instance.movingTiles++;
            coroutine = StartCoroutine(moveMent2D(dir));

            //위 하강 등록
            foreach (ITile tile in aboveTile) if (tile != null && !tile.isDrop) tile.Drop();

            return true;

        }

        return false;
    }

    IEnumerator moveMent2D(Vector2 direction)
    {

        yield return null;
        isDrop = false;

        // 현재 위치를 그리드에 정렬
        Vector2 startPos = new Vector2(
            Mathf.Round(transform.position.x / Utils.TILE_GAP) * Utils.TILE_GAP,
            Mathf.Round(transform.position.y / Utils.TILE_GAP) * Utils.TILE_GAP
        );

        Vector2 targetPos = startPos + direction * Utils.TILE_GAP;

        float time = 0f;
        while (time <= 1f)
        {
            transform.position = Vector2.Lerp(startPos, targetPos, time);
            time += Time.deltaTime * Utils.MOVEMENT_SPEED;
            yield return null;
        }

        // 위치 정밀 보정
        transform.position = targetPos;
        coroutineEnd();
    }

    protected void coroutineEnd()
    {
        EventManager.Instance.movingTiles--;

        StopCoroutine(coroutine);
        coroutine = null;

        Drop();
    }

}

