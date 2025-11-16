using UnityEngine;

public abstract class TileAction : GetActiveObjectFromWorld
{
    protected Tile tile;
    public void Init(Tile tile)
    {
        this.tile = tile;

    }
    public virtual void Invoke()
    {

        if (tile == null)
        {
            tile = GetComponent<Tile>();

            if (tile == null)
            {
                Debug.LogError($"{gameObject.name}에게 Tile이 없습니다!");
                return;
            }

            Debug.LogWarning($"{gameObject.name}에 있는 {GetType()}가 Init되지 않았습니다. 자동으로 Init합니다.");
        }

        if (!tile.isActive)
        {
            // Debug.LogWarning($"{gameObject.name}에 있는{GetType()}가 비활성 상태이므로 Invoke를 무시합니다.");
            return;
        }

        OnInvoke();
    }

    virtual protected void OnInvoke()
    {
        Debug.LogWarning($"{gameObject.name}에 있는 {GetType()}가 Invoke되었지만 구성되지 않아 무시되었습니다.");
    }

}
