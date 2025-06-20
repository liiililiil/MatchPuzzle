using UnityEngine;

public class BlastAction : TileAction
{
    public override void Invoke()
    {
        Tile[] aboveTile = { GetTileFromWorld<Tile>(transform.up, true), GetTileFromWorld<Tile>(transform.up + transform.right, true), GetTileFromWorld<Tile>(transform.up - transform.right, true) };

        if (tile == null)
        {
            Tile tile = GetComponent<Tile>();

            if (tile == null)
            {
                Debug.LogError($"{gameObject.name}에게 Tile이 없습니다!");
                return;
            }

            Debug.LogWarning($"{gameObject.name}에 있는 {GetType()}가 Init되지 않았습니다. 자동으로 Init합니다.");
        }

        if (!tile.isActive)
        {
            Debug.LogWarning($"{gameObject.name}에 있는{GetType()}가 비활성 상태이므로 Invoke를 무시합니다.");
            return;
        }

        OnInvoke();

        foreach (Tile t in aboveTile) t?.Drop();
    }
}
