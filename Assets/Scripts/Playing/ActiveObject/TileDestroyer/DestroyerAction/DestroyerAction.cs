using UnityEngine;

public class DestroyerAction : MonoBehaviour
{
    protected TileDestroyer tileDestroyer;
    public void Init(TileDestroyer tileDestroyer)
    {
        this.tileDestroyer = tileDestroyer;
        // Debug.Log("초기화 성공!");
    }

    public virtual void Invoke()
    {
        if (tileDestroyer == null)
        {
            TileDestroyer tileDestroyer = GetComponent<TileDestroyer>();

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

        OnInvoke();
    }

    protected virtual void OnInvoke()
    {
        Debug.LogWarning($"{gameObject.name}에 있는 {GetType()}가 Invoke되었지만 구성되지 않아 무시되었습니다.");
    }
}
