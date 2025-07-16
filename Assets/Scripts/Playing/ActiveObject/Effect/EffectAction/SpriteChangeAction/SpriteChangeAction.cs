using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SpriteChangeAction : EffectAction
{
    [SerializeField]
    protected Sprite[] sheet;
    protected SpriteRenderer spriteRenderer;


    public int spriteCount { get; set; }

    private void Awake()
    {
        if (sheet.GetLength(0) == 0)
        {
            Debug.LogError("시트가 없습니다!");
        }
        else
        {
            spriteCount = sheet.GetLength(0);
        }
        
        // Debug.Log("지정완료!");

    }
    public override void Invoke()
    {
        if (effect == null)
        {
            effect = GetComponent<Effect>();

            if (effect == null)
            {
                Debug.LogError($"{gameObject.name}에게 Effect가 없습니다!");
                return;
            }

            Debug.LogWarning($"{gameObject.name}에 있는 {GetType()}가 Init되지 않았습니다. 자동으로 Init합니다.");
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

            if (spriteRenderer == null)
            {
                Debug.LogError($"{gameObject.name}에게 SpriteRenderer가 없습니다!");
                return;
            }
        }



        if (!effect.isActive)
        {
            Debug.LogWarning($"{gameObject.name}에 있는{GetType()}가 비활성 상태이므로 Invoke를 무시합니다. \n");
            return;
        }

        OnInvoke();
    }

}
