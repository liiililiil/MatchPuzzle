using UnityEngine;

// 메인 이펙트 클래스
public class Effect : MonoBehaviour, IActiveObject
{
    // 해당 이펙트가 어디서 시작됬는지 기록하기 위한 변수
    public IActiveObject startBy { get; private set; }

    // 이펙트의 타입
    public EffectType type;

    // 이펙트가 활성화 되었는지 여부
    public bool isActive { get; private set; }

    // 초기화 여부
    private bool isInit;
    
    // 액션들
    private IEffectExtinctionAction extinctionAction;
    private IEffectMovementAction effectMovementAction;
    private ISpriteChangeAction spriteChangeAction;

    // 제네릭으로 액션 초기화 함수 정의

    private void InitAction<T>(out T action) where T : IEffectAction
    {
        action = GetComponent<T>();

        if (action == null)
            Debug.LogError($"{gameObject.name} 에게 {typeof(T)} 컴포넌트가 없습니다!");
        else if (action is IEffectAction)
            action.Init(this);
    }


    // 이펙트 활성화 함수
    public void Enable(Vector2 position, Quaternion rotate, IActiveObject startBy = null)
    {
        // 초기화가 한번도 안되었으면 초기화
        if (!isInit)
        {
            isInit = true;
            InitAction(out effectMovementAction);
            InitAction(out extinctionAction);
            InitAction(out spriteChangeAction);

        }

        if (startBy == null) Debug.LogWarning("이 타일 소멸자의 시작지점이 설정되지 못하였습니다!");

        // 위치와 방향 설정
        transform.position = position;
        transform.rotation = rotate;

        isActive = true;
    
        // 개체의 콜라이더 활성화
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        if (boxCollider2D != null)
            boxCollider2D.enabled = true;
        else
            Debug.LogError($"{gameObject.name} 에게 BoxCollider2D가 없습니다!");

        // 매인 액션 실행
        extinctionAction.Invoke();
        effectMovementAction.Invoke();
        spriteChangeAction.Invoke();

        //스폰 알림
        EventManager.Instance.OnSpawnedActiveOjbect.Invoke(this, transform.position);
    }

    // 이펙트 비활성화 함수
    public void Disable(bool hideEffect = false)
    {
        isActive = false;

        // 콜라이더 비활성화
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        if (boxCollider2D != null)
            boxCollider2D.enabled = false;
        else
            Debug.LogError($"{gameObject.name} 에게 BoxCollider2D가 없습니다!");

        // 이펙트를 대기 위치로 이동 후 회전 초기화
        transform.position = new Vector2(Utils.WAIT_POS_X, Utils.WAIT_Pos_Y);
        transform.rotation = Quaternion.identity;

        // 개체 풀링 시키기
        SpawnManager.Instance.Pooling(type, gameObject);
    }

}
