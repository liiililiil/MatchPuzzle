using UnityEngine;

public class DestroyerDestroyAction : DestroyerAction
{
    protected BoxCollider2D boxCollider2D;

    public override void Invoke()
    {
        if (tileDestroyer == null)
        {
            tileDestroyer = GetComponent<TileDestroyer>();

            if (tileDestroyer == null)
            {
                Debug.LogError($"{gameObject.name}에게 Destroyer가 없습니다!");
                return;
            }

            Debug.LogWarning($"{gameObject.name}에 있는 {GetType()}가 Init되지 않았습니다. 자동으로 Init합니다.");
        }

        if (!tileDestroyer.isActive)
        {
            Debug.LogWarning($"{gameObject.name}에 있는{GetType()}가 비활성 상태이므로 Invoke를 무시합니다.");
            return;
        }

        if( boxCollider2D == null)
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
            if (boxCollider2D == null) Debug.LogError("Box Collider가 없습니다!");
        }

        OnInvoke();
    }
}
