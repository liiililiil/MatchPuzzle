using UnityEngine;

public class TileDestroyer : MonoBehaviour, IActiveObject
{
    public TileType startBy { get; private set; }
    public DestroyerType type;
    public bool isActive { get; set; }

    private IMovementAction movementAction;
    private IDestroyAction destroyAction;
    private IExtinctionAction extinctionAction;

    private void Awake()
    {
        InitAction(out movementAction, "IMovementAction");
        InitAction(out destroyAction, "IDestroyAction");
        InitAction(out extinctionAction, "IExtinctionAction");
    }
    public void Fire(Tile startBy, Vector2 pos, Quaternion rotate)
    {
        // this.startBy = startBy;
        transform.position = pos;
        transform.rotation = rotate;
    }

    private void InitAction<T>(out T action, string actionName) where T : IDestroyerAction
    {
        action = GetComponent<T>();
        if (action == null)
        {
            Debug.LogError($"{gameObject.name} 에게 {actionName} 컴포넌트가 없습니다!");
        }
        else if (action is IDestroyerAction)
        {
            action.Init(this);
        }
    }

    public void Enable(Vector2 position, Quaternion rotate, IActiveObject startBy = null)
    {
        if(startBy == null) Debug.LogWarning("이 타일 소멸자의 시작지점이 설정되지 못하였습니다!");
        transform.position = position;
        transform.rotation = rotate;
        isActive = true;

        movementAction?.Invoke();
        destroyAction?.Invoke();
        extinctionAction?.Invoke();

    }
    
    public void Disable()
    {
        isActive = false;

        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        if (boxCollider2D != null)
            boxCollider2D.enabled = false;
        else
            Debug.LogError($"{gameObject.name} 에게 BoxCollider2D가 없습니다!");

        EventManager.Instance.OnSpawnedActiveOjbect.Invoke(this, transform.position);

        transform.position = new Vector2(Utils.WAIT_POS_X, Utils.WAIT_Pos_Y);
        transform.rotation = Quaternion.identity;
        // SpawnManager.Instance.pooling(gameObject, this);
    }

}
