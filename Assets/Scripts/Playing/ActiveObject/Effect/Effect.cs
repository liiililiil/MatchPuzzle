using System.Collections;
using UnityEngine;

public class Effect : MonoBehaviour, IActiveObject
{

    public IActiveObject startBy { get; private set; }
    public EffectType type;
    public bool isActive { get; private set; }
    private bool isInit;

    private IEffectExtinctionAction extinctionAction;
    private IEffectMovementAction effectMovementAction;
    private ISpriteChangeAction spriteChangeAction;

    private void InitAction<T>(out T action) where T : IEffectAction
    {
        action = GetComponent<T>();
        
        if (action == null)
        {
            Debug.LogError($"{gameObject.name} 에게 {typeof(T)} 컴포넌트가 없습니다!");
        }
        else if (action is IEffectAction)
        {
            action.Init(this);
        }
    }

    public void Enable(Vector2 position, Quaternion rotate, IActiveObject startBy = null)
    {
        if (!isInit)
        {
            isInit = true;
            InitAction(out effectMovementAction);
            InitAction(out extinctionAction);
            InitAction(out spriteChangeAction);
        }
        
        if (startBy == null) Debug.LogWarning("이 타일 소멸자의 시작지점이 설정되지 못하였습니다!");
        transform.position = position;
        transform.rotation = rotate;
        isActive = true;

        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        if (boxCollider2D != null)
            boxCollider2D.enabled = true;
        else
            Debug.LogError($"{gameObject.name} 에게 BoxCollider2D가 없습니다!");

        extinctionAction.Invoke();
        effectMovementAction.Invoke();
        spriteChangeAction.Invoke();

        EventManager.Instance.OnSpawnedActiveOjbect.Invoke(this, transform.position);
    }
    
    public void Disable(bool hideEffect = false)
    {
        // Debug.Log("해제 선언됨");
        isActive = false;
        
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        if (boxCollider2D != null)
            boxCollider2D.enabled = false;
        else
            Debug.LogError($"{gameObject.name} 에게 BoxCollider2D가 없습니다!");

        transform.position = new Vector2(Utils.WAIT_POS_X, Utils.WAIT_Pos_Y);
        transform.rotation = Quaternion.identity;

        SpawnManager.Instance.Pooling(type, gameObject);
    }

}
