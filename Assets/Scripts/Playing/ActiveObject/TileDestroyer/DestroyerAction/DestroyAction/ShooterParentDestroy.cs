
using System.Collections;
using System.Linq;
using UnityEngine;

public class ShooterParentDestroy : DestroyerDestroyAction, IDestroyAction
{
    private TileType targetTile = TileType.Empty;
    private TileType switchTile = TileType.Empty;
    protected override void OnInvoke()
    {
        Tile startByTemp = tileDestroyer.startBy as Tile;

        //변경된 타일이 색 타일이 아니면 임의의 색타일을 선정합니다.
        //만약 컬러 폭탄이랑 교체되었다면 모든 타일을 목표로 선정합니다.
        if (startByTemp.tileType != TileType.ColorBomb)
        {
            switchTile = startByTemp.tileType;
            targetTile = (TileType)Random.Range((byte)TileType.Red, (byte)TileType.Purple + 1);
        }
        else
        {
            if (startByTemp.switchedTileType == TileType.ColorBomb)
            {
                targetTile = TileType.Empty;
            }
            else if (!TILE_CONSTANT.COLOR_TILES.Contains(startByTemp.switchedTileType))
            {
                targetTile = (TileType)Random.Range((byte)TileType.Red, (byte)TileType.Purple + 1);
                // Debug.Log($"타일 선택이 안되어있어 랜덤으로 {targetTile}가 선택되었습니다.");
            }
            else
            {
                targetTile = startByTemp.switchedTileType;
            }

            if(startByTemp.switchedTileType != TileType.ColorBomb && !TILE_CONSTANT.COLOR_TILES.Contains(startByTemp.tileType))
            {
                switchTile = startByTemp.switchedTileType;
            }
            else
            {
                switchTile = TileType.Empty;
            }


        }

        Debug.Log($"파괴자가 {targetTile} 타일을 파괴합니다. 교체 타일 : {switchTile}", this);

        boxCollider2D.enabled = true;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Tile tile = collision.gameObject.GetComponent<Tile>();


        if (targetTile != TileType.Empty && tile.tileType != targetTile)
        {
            return;
        }

        // if(targetTile == TileType.ColorBomb)
        // {
        //     return;
        // }


        //카메라 밖에 있으면 무시
        Vector3 viewPos = Camera.main.WorldToViewportPoint(collision.transform.position);

        if (viewPos.z < 0 || viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
            return;

        StartCoroutine(disableColliderTimer(tile));
        SpawnManager.Instance.SpawnObject(EffectType.ShooterChildEffect, transform.position, Quaternion.identity, tileDestroyer.startBy).GetComponent<ShooterChildMovement>().targetPosition = collision.transform.position;
    }

    IEnumerator disableColliderTimer(Tile tile)
    {
        // 일정 시간 대기
        float time = 0f;
        while (time <= 1)
        {
            time += Time.deltaTime * Utils.SHOOTER_CHILD_MOVE_SPEED;
            yield return null;
            // Debug.Log(time);
        }

        // 활성화 안되여있으면 무시
        if(tile.isActive == false)
        {
            yield break;
        }

        // 타일 스폰 후 즉시 폭발시키기 위해 저장
        Tile temp = null;
        if (switchTile != TileType.Empty)
        {
            temp = SpawnManager.Instance.SpawnObject(switchTile, tile.transform.position, Quaternion.identity).GetComponent<Tile>();
        }

        tile.hit(tileDestroyer);

        if (temp != null) temp.hit(tileDestroyer);

        yield return null;
    }
}
