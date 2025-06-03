using System.Collections;
using UnityEngine;

public abstract class DropTile : Tile, ITile
{
    public void FixedUpdate()
    {
        if (rigidbody2D == null) rigidbody2D = GetComponent<Rigidbody2D>();

        Vector2 dir = -transform.up.normalized;
        rigidbody2D.AddForce(9.81f * dir * Time.fixedDeltaTime * 0.01f, ForceMode2D.Force);

        // Debug.Log(rigidbody2D.totalForce);
    }
    


    //사용하지 않음 (명시 연산)
    // private bool isMoving = false;
    // private float acceleration = 1;
    // public override sealed void Drop()
    // {
    //     //이미 이동중인데 함수 호출되면 에러 반환
    //     if (isMoving) throw new System.Exception("움직이는 중 하강 연산이 요청되었습니다!");

    //     acceleration = 1;

    //     if (circleCollider2D == null) circleCollider2D = this.GetComponent<CircleCollider2D>();


    //     DropCalculation();
    // }

    // protected void DropCalculation()
    // {

    //     //자신의 밑 타일이 없다면
    //     ITile belowTile = Raycast(Vector2.down, 1, true);
    //     Debug.Log(belowTile);

    //     if (belowTile == null)
    //     {
    //         //일단 위에 타일 구하기
    //         ITile[] aboveTile = { Raycast(Vector2.up, 1, true), Raycast(Vector2.up + Vector2.right, 1.7f, true), Raycast(Vector2.up + Vector2.left, 1.7f, true) };

    //         //이동
    //         StartCoroutine(moveMent2D((-transform.up.normalized * Utils.TILE_GAP) + transform.position));

    //         //위 하강 등록
    //         foreach (ITile tile in aboveTile) tile?.Drop();


    //         //끝내기
    //         return;
    //     }

    //     //옆으로 하강 연산
    //     foreach (Vector2 dir in Utils.xDirections)
    //     {
    //         belowTile = Raycast(Vector2.down + dir, 1.7f, true);
    //         Debug.Log(belowTile);

    //         if (belowTile == null)
    //         {
    //             //일단 위에 타일 구하기
    //             ITile[] aboveTile = { Raycast(Vector2.up, 1, true), Raycast(Vector2.up + Vector2.right, 1.7f, true), Raycast(Vector2.up + Vector2.left, 1.7f, true) };

    //             Vector3 localDir;
    //             if (dir == Vector2.left) localDir = -transform.right;
    //             else localDir = transform.right;

    //             //이동
    //             StartCoroutine(moveMent2D(((-transform.up.normalized + localDir) * Utils.TILE_GAP) + transform.position));

    //             //위 하강 등록
    //             foreach (ITile tile in aboveTile) tile?.Drop();

    //             //끝내기
    //             return;
    //         }

    //     }


    //     //떨어지지 않았다면 콜라이더 켜기
    //     circleCollider2D.enabled = true;
    // }

    // IEnumerator moveMent2D(Vector2 targetPos)
    // {
    //     circleCollider2D.enabled = false;
    //     // 원래 위치 저장
    //     Vector2 startPos = transform.position;
    //     float time = 0;
    //     isMoving = true;

    //     while (time <= 1)
    //     {
    //         transform.position = Vector2.Lerp(startPos, targetPos, time);
    //         time += Time.deltaTime * Utils.MOVEMENT_SPEED * acceleration;
    //         acceleration += 0.1f;

    //         // Debug.Log(time);

    //         yield return null;
    //     }

    //     //이동 완료후 위치 교정
    //     transform.position = targetPos;
    //     isMoving = false;

    //     DropCalculation();
    // }

}

