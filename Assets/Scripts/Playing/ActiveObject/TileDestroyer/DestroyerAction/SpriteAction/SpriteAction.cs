using UnityEngine;

public class SpriteAction : DestroyerAction, ISpriteAction
{
    [SerializeField]
    protected Sprite[] sheet;
    protected SpriteRenderer spriteRenderer;

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


        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();

            if (spriteRenderer == null)
            {
                Debug.LogError($"{gameObject.name}에게 SpriteRenderer가 없습니다!");
                return;
            }
        }

        if (!tileDestroyer.isActive)
        {
            Debug.LogWarning($"{gameObject.name}에 있는{GetType()}가 비활성 상태이므로 Invoke를 무시합니다.");
            return;
        }

        OnInvoke();
    }

    public int GetLenght(){
        return sheet.Length;
    }

    
}
