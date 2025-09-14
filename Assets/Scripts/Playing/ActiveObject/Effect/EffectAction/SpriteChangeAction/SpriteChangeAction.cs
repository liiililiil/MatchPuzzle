using UnityEngine;

// 스프라이트 변경을 위한 기본 클래스
public class SpriteChangeAction : EffectAction
{
    // 재생할 스프라이트 시트
    [SerializeField]
    protected Sprite[] sheet;


    protected SpriteRenderer spriteRenderer;

    // 재생될 스프라이트 개수
    public byte spriteCount { get; set; }

    // 시트의 수를 저장
    private void Awake()
    {
        // 시트가 없으면 에러 반환
        if (sheet.GetLength(0) == 0)
            Debug.LogError("시트가 없습니다!");
        else
            spriteCount = (byte)sheet.GetLength(0);
    }


    public override void Invoke()
    {
        // 필요한 개체들을 가져온 후 메인 로직 실행
        
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

            Debug.LogWarning($"{gameObject.name}에 있는 {GetType()}가 Init되지 않았습니다. 자동으로 Init합니다.");
        }



        if (!effect.isActive)
        {
            Debug.LogWarning($"{gameObject.name}에 있는{GetType()}가 비활성 상태이므로 Invoke를 무시합니다. \n");
            return;
        }

        OnInvoke();
    }

}
