using UnityEngine;

[System.Serializable]
public class ExplosionType
{
    public DestroyerType type;
    public float rotate;
}


public class ExplosionBlast : BlastAction, IBlastAction
{
    [SerializeField]
    ExplosionType[] targetDestroyers;

    protected override void OnInvoke()
    {
        foreach (ExplosionType target in targetDestroyers) 
        {
            // Debug.Log(target.rotate);
            SpawnManager.Instance.SpawnObject(target.type, transform.position, Quaternion.Euler(0, 0, target.rotate + transform.rotation.z), tile);
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
