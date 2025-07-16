using UnityEngine;

public class IsCanChangeBombBlast : BlastAction, IBlastAction
{
    protected override void OnInvoke()
    {
        EventManager.Instance.OnBlastTile.Invoke(tile, transform.position);

        if (tile.isCenter) BombSpawn();

        CallOrganize();


        tile.Disable();
    }

    private void BombSpawn()
    {
        if (tile.length.x >= 4 || tile.length.y >= 4)
        {
            SpawnManager.Instance.SpawnObject(TileType.ColorBomb, transform.position, transform.rotation);
        }
        else if (tile.length.x >= 2 && tile.length.y >= 2)
        {
            SpawnManager.Instance.SpawnObject(TileType.BigBomb, transform.position, transform.rotation);
        }
        else if (tile.length.x >= 3)
        {
            SpawnManager.Instance.SpawnObject(TileType.XBomb, transform.position, transform.rotation);
        }
        else if (tile.length.y >= 3)
        {
            SpawnManager.Instance.SpawnObject(TileType.YBomb, transform.position, transform.rotation);
        }
        else
        {
            // Debug.Log("폭탄 생성 조건에 해당하지 않음");
            // Debug.Log(tile.length);
        }
        
    }
    
    private void CallOrganize()
    {
        //4 방향으로 Ray 쏘기
        foreach (Vector2 dir in Utils.directions)
        {
            // 상대 방향을 절대 방향으로 변환
            Vector2 worldDir = dir.x * (Vector2)transform.right + dir.y * (Vector2)transform.up;

            // GetTileFromWorld를 이용해 타일을 찾고 정리 수행
            Tile tile = GetTileFromWorld<Tile>(dir, true);
            tile?.Organize();
        }

    }
    
}
