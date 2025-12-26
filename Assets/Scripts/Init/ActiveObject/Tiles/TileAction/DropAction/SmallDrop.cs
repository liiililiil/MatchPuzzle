using System.Collections;
using UnityEngine;

// 1X1 타일 드랍 액션
public class SmallDrop : DropAction, IDropAction
{

    // 드랍 가능
    private TileMovement movement = null;
    public bool isCanDrop { get { return true; } }
    protected override void OnInvoke()
    {

        if (movement != null && movement.enabled == true)
        {
            return;
        }

        //밑으로 하강 연산
        if (!DropCheck(Vector2.zero))
        {
            //옆으로 하강 연산
            foreach (Vector2 dir in Utils.xDirections)
            {
                //상대 좌표를 절대 좌표로 변환
                Vector2 worldDir = dir == Vector2.right ? transform.right : -transform.right;

                if (DropCheck(worldDir)) return;
            }
    
        }
        else
        {
            return;
        }
    

        tile.Calculate();
    }

    private bool DropCheck(Vector2 dir)
    {
        //밑에 타일이 있는지 검사
        IDropAction belowTile = Utils.TryGetAction<IDropAction>(gameObject.transform.position, (Vector2)(-transform.up) + dir, Utils.TILE_GAP);

        //밑에 타일이 없으면
        if (belowTile == null)
        {
            //바로 위면 그냥 하강
            if(dir == Vector2.zero)
            {
                Drop(belowTile, -transform.up);
                return true;
            }
            //떨어질 위치
            Vector2 dropPoint = gameObject.transform.position +(((Vector3)dir - transform.up) * Utils.TILE_GAP);

            

            //떨어질 곳 위 탐색
            IDropAction dropAbove = Utils.TryGetAction<IDropAction>(dropPoint, transform.up, Utils.TILE_GAP);

            //떨어질 곳위에 뭔가 있고 뭔가가 떨어질 수 있으면 패스
            if(dropAbove != null &&dropAbove.isCanDrop == true) return false;

            // 옆 타일
            dropAbove = Utils.TryGetAction<IDropAction>(dropPoint, transform.up + (Vector3)dir, Utils.TILE_GAP); 

            // 왼쪽 타일이 우선적으로 떨어지게
            if(dir == (Vector2)transform.right)
            {
                if(dropAbove != null && dropAbove.isCanDrop == true) return false;
            }
            else
            {
                // if(dropAbove != null && dropAbove.isCanDrop == true) return false;
            }

            Drop(belowTile, (Vector2)(-transform.up) + dir);
            return true;
        }

        return false;
    }

    //하강
    protected void Drop(IDropAction belowTile, Vector2 dir)
    {

        Vector2 startPos = transform.position;

        //이동 위치
        Vector2 targetPos = startPos + dir * Utils.TILE_GAP;

        //위 하강 예약
        Tile[] tiles = new Tile[] {
            Utils.TryGetTile(transform.position, transform.up, Utils.TILE_GAP, true), 
            Utils.TryGetTile(transform.position, transform.up + transform.right, Utils.TILE_GAP, true), 
            Utils.TryGetTile(transform.position, transform.up - transform.right, Utils.TILE_GAP, true),
            tile
        };

        //히트박스는 미리 이동해놓기 
        tile.rigidbody2D.MovePosition(targetPos);

        ActiveateMovement(startPos, targetPos);

        // Debug.Log($"{tiles[0]}, {tiles[1]}, {tiles[2]}가 {tile}의 하강을 위해 예약되었습니다.", this);
        // 하강
        foreach (Tile tile in tiles) tile?.Drop();
    }

    private void ActiveateMovement(Vector2 startPos, Vector2 targetPos)
    {
        if (movement == null) movement = tile.gameObject.AddComponent<TileMovement>();
        movement.SetPosition(startPos, targetPos);
    }

}
