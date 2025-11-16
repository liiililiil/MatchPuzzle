using UnityEngine;


//다른 타일 타입과 조합될 때 생성하는 파괴자들
[System.Serializable]
public class CombineExplosionType
{
    public TileType comineBy;
    public DestroyerSpawnPreset[] explosions;
}


// 직접적으로 폭발하는 액션
public class ExplosionBlast : BlastAction, IBlastAction
{
    [SerializeField]
    DestroyerSpawnPreset[] targetDestroyers;

    [SerializeField]
    CombineExplosionType[] combineExplosionDestroyers;

    protected override void OnInvoke()
    {
        //움직였지만 메인 포커싱 타일이 아닌겨우 폭발대신 바로 비활성화
        if (tile.switched && tile.switchedTileType == TileType.Empty)
        {
            tile.Disable(true);
            return;
        }

        //조합되는 폭발 검사
        foreach (CombineExplosionType combine in combineExplosionDestroyers)
        {
            if (tile.switchedTileType == combine.comineBy)
            {
                SpawnDestroyers(combine.explosions);
                return;
            }
        }

        SpawnDestroyers(targetDestroyers);



    }

    //파괴자 배열을 받아서 전부 스폰
    private void SpawnDestroyers(DestroyerSpawnPreset[] targetList)
    {
        //파괴자 스폰
        foreach (DestroyerSpawnPreset target in targetList)
            foreach (ExplosionType destroyer in target.Destroyers)
                SpawnManager.Instance.SpawnObject(destroyer.type, transform.position + (destroyer.pos.x * transform.right + destroyer.pos.y * transform.up), Quaternion.Euler(0, 0, destroyer.rotate + transform.rotation.z), tile);

        //스폰후 본인은 정리 체크 후 비활성화
        CallOrganize();
        tile.Disable();
    }

    //4방향으로 타일을 검사하여 정리 요청
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
