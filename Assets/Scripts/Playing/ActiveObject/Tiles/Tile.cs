using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;



public class Tile : MonoBehaviour, IActiveObject
{
    private enum TileFlag : byte
    {
        isActive = 1 << 0,
        isCenter = 1 << 1,
    }

    private TileFlag flag;
    private ICalculateAction calculateAction;
    private IBlastAction blastAction;
    private IDropAction dropAction;
    private IOrganizeAction organizeAction;

    public bool isActive
    {
        get => (flag & TileFlag.isActive) != 0;
        set => flag = value ? flag | TileFlag.isActive : flag & ~TileFlag.isActive;
    }

    public bool isCenter
    {
        get => (flag & TileFlag.isCenter) != 0;
        set => flag = value ? flag | TileFlag.isCenter : flag & ~TileFlag.isCenter;
    }
    public Vector2Int length = Vector2Int.zero;

    public GameObject sprite;
    public TileType tileType;

    public bool DropTest;
    public bool calTest;


    private void Update()
    {
        isActive = true;
        if (DropTest)
        {
            DropTest = false;
            isCenter = true;
            Drop();
        }

        if (calTest)
        {
            calTest = false;
            Calculate();
        }
    }


    private void Awake()
    {
        InitAction(out calculateAction, "ICalculateAction");
        InitAction(out blastAction, "IBlastAction");
        InitAction(out dropAction, "IDropAction");
        InitAction(out organizeAction, "IOrganizeAction");
    }

    private void InitAction<T>(out T action, string actionName) where T : ITileAction
    {
        action = GetComponent<T>();
        if (action == null)
        {
            Debug.LogError($"{gameObject.name} 에게 {actionName} 컴포넌트가 없습니다!");
        }
        else if (action is ITileAction)
        {
            action.Init(this);
        }
    }

    private void Start()
    {
        if (sprite == null)
        {
            sprite = transform.GetChild(0).gameObject;
            if (sprite == null)
                Debug.LogError($"{gameObject.name} 에게 자식 sprite가 없습니다!");
            else
                Debug.LogWarning($"{gameObject.name} 에게 자식 sprite가 자동으로 할당되었습니다.");
        }

    }

    public void hit(TileDestroyer tileDestroyer)
    {
        EventManager.Instance.OnBlastTileByBomb.Invoke(this, tileDestroyer, transform.position);

        Disable();
    }

    public void Disable()
    {
        isActive = false;

        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        if (boxCollider2D != null)
            boxCollider2D.enabled = false;
        else
            Debug.LogError($"{gameObject.name} 에게 BoxCollider2D가 없습니다!");

        EventManager.Instance.OnDisabledTile.Invoke(this, transform.position);


        transform.position = new Vector2(Utils.WAIT_POS_X, Utils.WAIT_Pos_Y);

        if (sprite != null)
            sprite.SetActive(false);
        else
            Debug.LogError($"{gameObject.name} 에게 sprite가 없습니다!");

        transform.rotation = Quaternion.identity;
        SpawnManager.Instance.Pooling(tileType, gameObject);
    }

    public void Enable(Vector2 position, Quaternion rotate, IActiveObject startBy = null)
    {
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        if (boxCollider2D != null)
            boxCollider2D.enabled = true;
        else
            Debug.LogError($"{gameObject.name} 에게 BoxCollider2D가 없습니다!");

        if (sprite != null)
            sprite.SetActive(true);
        else
            Debug.LogError($"{gameObject.name} 에게 sprite가 없습니다!");

        transform.position = position;
        transform.rotation = rotate;

        EventManager.Instance.OnSpawnedActiveOjbect.Invoke(this, transform.position);

        isActive = true;

        Drop();
    }


    public void Calculate()
    {
        EventManager.Instance.InvokeCalculate += calculateAction.Invoke;
    }
    public void Drop()
    {
        EventManager.Instance.InvokeDrop += dropAction.Invoke;
    }
    public void Blast()
    {
        EventManager.Instance.InvokeBlast += blastAction.Invoke;

    }

    public void Organize()
    {
        EventManager.Instance.InvokeOrganize += organizeAction.Invoke;
    }

    public void CalReset()
    {
        EventManager.Instance.InvokeCalReset += calculateAction.CalReset;
    }

    public void OnplayerFocus()
    {
        StartCoroutine(OnPlayerFocusCoroutine());
    }

    IEnumerator OnPlayerFocusCoroutine()
    {
        Vector2 pos = transform.position;

        while (Input.GetMouseButton(0))
        {
            Vector2 mousePos= Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float dis = -Vector2.Distance(pos, mousePos) + 1;
            sprite.transform.position = Vector2.Lerp(sprite.transform.position, mousePos * dis,0.8f);
            yield return null;
        }
        
    }
}
