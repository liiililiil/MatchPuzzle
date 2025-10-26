using UnityEngine;

// 타일 파괴자 클래스
public class TileDestroyer : MonoBehaviour, IActiveObject
{
    // 시작된 위치
    public IActiveObject startBy { get; private set; } = null;

    // 타일 파괴자 타입
    public DestroyerType type;

    // 활성화 여부
    public bool isActive { get; private set; }

    // 초기화 여부
    private bool isInit = false;

    // 액션들
    private IMovementAction movementAction;
    private IDestroyAction destroyAction;
    private IExtinctionAction extinctionAction;
    private ISpriteAction spriteAction;
    private IEffectSpawnAction effectAction;

    // 제네릭으로 액션 초기화 함수 정의
    private void InitAction<T>(out T action) where T : IDestroyerAction
    {
        action = GetComponent<T>();
        if (action == null)
        {
            Debug.LogWarning($"{gameObject.name} 에게 {typeof(T)} 컴포넌트가 없습니다!");
        }
        else if (action is IDestroyerAction)
        {
            action.Init(this);
        }
    }

    //활성화
    public void Enable(Vector2 position, Quaternion rotate, IActiveObject startBy = null)
    {
        // if(type == DestroyerType.ShooterParent) Debug.Log("제ㅔㅔㅔㅔㅔ발 ",this);
        EventManager.Instance.activeDestroyer++;

        //초기화 되지 않았으면 초기화
        if (!isInit)
        {
            isInit = true;
            InitAction(out movementAction);
            InitAction(out destroyAction);
            InitAction(out extinctionAction);
            InitAction(out spriteAction);
            InitAction(out effectAction);
        }
        
        // if (type == DestroyerType.ShooterParent) Debug.Log("초기화 ");
        
        if (startBy == null) Debug.LogWarning("이 타일 소멸자의 시작지점이 설정되지 못하였습니다!",this);
            else if (!(startBy is Tile)) Debug.LogWarning("이 타일 소멸자의 시작지점이 타일이 아닙니다!",this);
            else this.startBy = startBy;

        // 위치 및 방향 설정
        transform.position = position;
        transform.rotation = rotate;


        isActive = true;

        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        if (boxCollider2D != null)
            boxCollider2D.enabled = true;
        else
            Debug.LogError($"{gameObject.name} 에게 BoxCollider2D가 없습니다!");

        //스폰 알림
        EventManager.Instance.OnSpawnedActiveOjbect.Invoke(this, transform.position);

        // if (type == DestroyerType.ShooterParent) Debug.Log($"인보크 {destroyAction.GetType()} ",this);
        //액션 실행
        movementAction?.Invoke();
        destroyAction?.Invoke();
        extinctionAction?.Invoke();
        spriteAction?.Invoke();
        effectAction?.Invoke();

    }

    //비활성화
    public void Disable(bool hideEffect = false)
    {
        EventManager.Instance.activeDestroyer--;

        isActive = false;

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
