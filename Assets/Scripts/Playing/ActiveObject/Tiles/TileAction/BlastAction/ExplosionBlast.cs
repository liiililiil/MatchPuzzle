using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class ExplosionType
{
    public DestroyerType type;
    public Vector2 pos;
    public float rotate;
}

[System.Serializable]
public class CombineExplosionType
{
    public TileType comineBy;
    public ExplosionType[] explosions;
}


public class ExplosionBlast : BlastAction, IBlastAction
{
    [SerializeField]
    ExplosionType[] targetDestroyers;

    [SerializeField]
    CombineExplosionType[] combineExplosionDestroyers;

    protected override void OnInvoke()
    {
        //움직였지만 메인 포커싱 타일이 아닌겨우 폭발대신 바로 비활성화
        if(tile.switched && tile.switchedTileType == TileType.Empty)
        {
            tile.Disable(true);
            return;
        }

        foreach (CombineExplosionType combine in combineExplosionDestroyers)
        {
            // Debug.Log(combine.rotate);
            if (tile.switchedTileType == combine.comineBy)
            {
                SpawnDestroyers(combine.explosions);
                return;
            }
        }

        SpawnDestroyers(targetDestroyers);



    }

    private void SpawnDestroyers(ExplosionType[] targetList)
    {
        Debug.Log(targetList);
        foreach (ExplosionType target in targetList)
        {
            // Debug.Log(target.rotate);
            SpawnManager.Instance.SpawnObject(target.type, transform.position + (target.pos.x * transform.right + target.pos.y * transform.up), Quaternion.Euler(0, 0, target.rotate + transform.rotation.z), tile);
        }

        CallOrganize();
        tile.Disable();
    }


    private void CallOrganize()
    {
        //4 방향으로 Ray 쏘기
        foreach (Vector2 dir in Utils.directions)
        {
            // 상대 방향을 절대 방향으로 변환
            Vector2 worldDir = dir.x * (Vector2)transform.right + dir.y * (Vector2)transform.up;

            // GetTileFromWorld를 이용해 타일을 찾고 정리 수행
            Tile tile = GetTileFromWorld<Tile>(worldDir);
            tile?.Organize();
        }

    }

}
