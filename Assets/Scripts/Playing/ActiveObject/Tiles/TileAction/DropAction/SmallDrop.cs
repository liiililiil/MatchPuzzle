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
            Debug.LogWarning("이미 움직이는 중이라 무시되었습니다.");
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

            ActiveateMovement(startPos, targetPos);

            // 하강
            foreach (Tile tile in tiles) if (tile != null) tile.Drop();

            return true;

        }

        return false;
    }

    private void ActiveateMovement(Vector2 startPos, Vector2 targetPos)
    {
        if (movement == null) movement = tile.gameObject.AddComponent<TileMovement>();
        movement.SetPosition(startPos, targetPos);
    }

}
