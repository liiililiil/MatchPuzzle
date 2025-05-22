using System.Collections;
using UnityEngine;

public abstract class DropTile : Tile, ITile
{
    private bool isMoving = false;
    private float acceleration = 1;
    public override sealed void Drop()
    {
        acceleration = 1;
        DropCalculation();
    }

    protected void DropCalculation()
    {
        //이미 이동중인데 함수 호출되면 에러 반환
        if (isMoving) throw new System.Exception("움직이는 중 하강 연산이 요청되었습니다!");
        //자신의 밑 타일이 없다면
        ITile belowTile = Raycast(Vector2.down, 1, true);

        if (belowTile == null)
        {
            //일단 위에 타일 구하기
            ITile aboveTile = Raycast(Vector2.up, 1, true);

            //이동
            StartCoroutine(moveMent2D((Vector2.down * Utils.TILE_GAP) + (Vector2)transform.position));

            //위 하강 등록
            aboveTile?.Drop();
        }
    }

    IEnumerator moveMent2D(Vector2 targetPos)
    {
        boxCollider2D.enabled = false;

        // 원래 위치 저장
        Vector2 startPos = transform.position;
        float time = 0;


        isMoving = true;

        
        while (time <= 1)
        {
            transform.position = Vector2.Lerp(startPos, targetPos, time);
            time += Time.deltaTime * Utils.MOVEMENT_SPEED * acceleration;
            acceleration += 0.1f;

            // Debug.Log(time);

            yield return null;
        }

        //이동 완료후 위치 교정
        transform.position = targetPos;

        boxCollider2D.enabled = true;

        isMoving = false;
        DropCalculation();
    }
}

