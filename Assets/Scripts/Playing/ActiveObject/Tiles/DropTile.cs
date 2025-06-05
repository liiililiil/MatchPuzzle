using System.Collections;
using UnityEngine;

public abstract class DropTile : Tile, ITile
{
    public override sealed void Drop()
    {
        //이미 이동중인데 함수 호출되면 반환
        if (isDrop) return;

        if (circleCollider2D == null) circleCollider2D = this.GetComponent<CircleCollider2D>();

        DropCalculation();
    }

    protected void DropCalculation()
    {

        //자신의  타일이 없다면
        ITile belowTile = Raycast(Vector2.down, 1, true);

        if (belowTile == null || belowTile.isDrop)
        {
            //일단 위에 타일 구하기
            ITile[] aboveTile = { Raycast(Vector2.up, 1, true), Raycast(Vector2.up + Vector2.right, 1.7f, true), Raycast(Vector2.up + Vector2.left, 1.7f, true) };

            //이동
            isDrop = true;
            StartCoroutine(moveMent2D(-transform.up));


            foreach (ITile tile in aboveTile) if (tile != null && !tile.isDrop) tile.Drop();

            //끝내기
            return;
        }
        
        //옆으로 하강 연산
        foreach (Vector2 dir in Utils.xDirections)
        {
            belowTile = Raycast(Vector2.down + dir, 1.7f, true);
            Debug.Log(belowTile);

            if (belowTile == null || belowTile.isDrop)
            {
                //옆으로 하강하기전에 떨어질 타일이 있으면 떨어지지않음
                ITile sideTile = Raycast(dir, 1, true);
                if (sideTile != null) continue;

                //일단 위에 타일 구하기
                ITile[] aboveTile = { Raycast(Vector2.up, 1, true), Raycast(Vector2.up + Vector2.right, 1.7f, true), Raycast(Vector2.up + Vector2.left, 1.7f, true) };

                //이동
                isDrop = true;
                StartCoroutine(moveMent2D(-transform.up + (dir == Vector2.right ? transform.right : -transform.right)));
                //위 하강 등록
                foreach (ITile tile in aboveTile) if (tile != null && !tile.isDrop) tile.Drop();

                //끝내기
                return;
            }
        }


    }

    IEnumerator moveMent2D(Vector2 direction)
    {
        // 방향 정규화
        // direction = direction.normalized;

        // 타겟 위치 계산 (절대 위치 기준)
        Vector2 startPos = transform.position;
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
        DropCalculation();
        
        isDrop = false;
    }

}

