using UnityEngine;

// 폭탄으로 변하는 폭발 액션
public class IsCanChangeBombBlast : BlastAction, IBlastAction
{
    protected override void OnInvoke()
    {
        EventManager.Instance.OnBlastTile.Invoke(tile, transform.position);

        if (tile.isCenter) BombSpawn();

        Utils.CallOrganize(gameObject);


        tile.Disable();
    }

    // 폭탄 조건 검사 후 생성
    private void BombSpawn()
    {
        // 컬러 폭탄
        if (tile.length.x >= 5 || tile.length.y >= 5)
        {
            SpawnManager.Instance.SpawnObject(TileType.ColorBomb, transform.position, transform.rotation);
        }

        // 큰 폭탄
        else if (tile.length.x >= 3 && tile.length.y >= 3)
        {
            SpawnManager.Instance.SpawnObject(TileType.BigBomb, transform.position, transform.rotation);
        }

        // 세로 폭탄
        else if (tile.length.x >= 4)
        {
            SpawnManager.Instance.SpawnObject(TileType.XBomb, transform.position, transform.rotation);
        }

        // 가로 폭탄
        else if (tile.length.y >= 4)
        {
            SpawnManager.Instance.SpawnObject(TileType.YBomb, transform.position, transform.rotation);
        }

        // 폭탄 생성 조건에 해당하지 않음
        else
        {
            // Debug.Log("폭탄 생성 조건에 해당하지 않음");
            // Debug.Log(tile.length);
        }

    }



}
