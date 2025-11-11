using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;



public class Tile : GetActiveObjectFromWorld, IActiveObject
{
    
    private enum TileFlag : byte
    {
        isActive = 1 << 0,
        isCenter = 1 << 1,
        isInit = 1 << 2,
        SWitched = 1 << 3
    }

    private TileFlag flag;
    private ICalculateAction calculateAction;
    private IBlastAction blastAction;
    private IDropAction dropAction;
    private IOrganizeAction organizeAction;
    private IExplodedAction explodedAction;
    private ITileFocusAction focusAction;

    
    public bool isActive
    {
        get => (flag & TileFlag.isActive) != 0;
        private set => flag = value ? flag | TileFlag.isActive : flag & ~TileFlag.isActive;
    }

    public bool isCenter
    {
        get => (flag & TileFlag.isCenter) != 0;
        set => flag = value ? flag | TileFlag.isCenter : flag & ~TileFlag.isCenter;
    }

    private bool isInit
    {
        get => (flag & TileFlag.isInit) != 0;
        set => flag = value ? flag | TileFlag.isInit : flag & ~TileFlag.isInit;
    }
    private bool _switched
    {
        get => (flag & TileFlag.SWitched) != 0;
        set => flag = value ? flag | TileFlag.SWitched : flag & ~TileFlag.SWitched;
    }

    //외부 접근용
    public bool switched
    {
        get => _switched;
        set
        {
            _switched = value;
        }
    }
    public Vector2Int length = Vector2Int.zero;
    public new Rigidbody2D rigidbody2D;

    public GameObject sprite;
    public TileType tileType;

    public TileType switchedTileType;

    public bool DropTest;
    public bool calTest;
    private void Update()
    {
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

    private void InitAction<T>(out T action) where T : ITileAction
    {
        action = GetComponent<T>();
        if (action == null)
        {
            Debug.LogError($"{gameObject.name} 에게 {typeof(T)} 컴포넌트가 없습니다! {action}" , this);
        }
        else if (action is ITileAction)
        {
            action.Init(this);
        }
    }
    private void Start()
    {
        this.Enable(transform.position, transform.rotation);
    }

    public void hit(TileDestroyer tileDestroyer)
    {
        EventManager.Instance.OnBlastTileByBomb.Invoke(this, tileDestroyer, transform.position);
        explodedAction.Invoke();
    }

    public void Disable(bool hideEffect = false)
    {
        rigidbody2D.simulated = false;
        rigidbody2D.Sleep();

        Tile[] aboveTile = { GetTileFromWorld<Tile>(transform.up), GetTileFromWorld<Tile>(transform.up + transform.right), GetTileFromWorld<Tile>(transform.up - transform.right) };

        isActive = false;
        _switched = false;

        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        if (boxCollider2D != null)
            boxCollider2D.enabled = false;
        else
            Debug.LogError($"{gameObject.name} 에게 BoxCollider2D가 없습니다!");

        EventManager.Instance.OnDisabledTile.Invoke(this, transform.position);
        if(!hideEffect) SpawnManager.Instance.SpawnObject(EffectType.TileDisabled, transform.position, transform.rotation, this);

        if (dropAction.isDrop == true)
        {
            dropAction.isDrop = false;
            EventManager.Instance.movingTiles--;
        }


        transform.position = new Vector2(Utils.WAIT_POS_X, Utils.WAIT_POS_Y);
        foreach (Tile t in aboveTile) t?.Drop();

        if (sprite != null)
            sprite.SetActive(false);
        else
            Debug.LogError($"{gameObject.name} 에게 sprite가 없습니다!");

        transform.rotation = Quaternion.identity;
        SpawnManager.Instance.Pooling(tileType, gameObject);
    }

    public void Enable(Vector2 position, Quaternion rotate, IActiveObject startBy = null)
    {
        if (!isInit)
        {
            isInit = true;

            InitAction(out calculateAction);
            InitAction(out blastAction);
            InitAction(out dropAction);
            InitAction(out organizeAction);
            InitAction(out explodedAction);
            InitAction(out focusAction);

            if (sprite == null)
            {
                sprite = transform.GetChild(0).gameObject;
                if (sprite == null)
                    Debug.LogError($"{gameObject.name} 에게 자식 sprite가 없습니다!");
                else
                    Debug.LogWarning($"{gameObject.name} 에게 자식 sprite가 자동으로 할당되었습니다.");
            }

            if (rigidbody2D == null)
            {
                rigidbody2D = GetComponent<Rigidbody2D>();
                if (rigidbody2D == null)
                    Debug.LogError($"{gameObject.name} 에게 Rigidbody2D가 없습니다!");
                // else
                // Debug.LogWarning($"{gameObject.name} 에게 Rigidbody2D가 자동으로 할당되었습니다.");
            }
        }

        switchedTileType = TileType.Empty;


        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        if (boxCollider2D != null)
            boxCollider2D.enabled = true;
        else
            Debug.LogError($"{gameObject.name} 에게 BoxCollider2D가 없습니다!");

        sprite.SetActive(true);
        transform.position = position;
        transform.rotation = rotate;

        EventManager.Instance.OnSpawnedActiveOjbect.Invoke(this, transform.position);

        isActive = true;

        rigidbody2D.simulated = true;

        Drop();
    }

    // public void focus()
    // {
    //     StartCoroutine(WhileFocus());
    // }

    // IEnumerator WhileFocus()
    // {
    //     Tile tile = null;
    //     boo

    //     // 타일 있는지 확인
    //     tile = GetTileFromWorld<Tile>(Vector2.up);

    //     if(tile != null && tile.is)

        
    // }


    public void Calculate()
    {
        if (isActive == false) return;
        EventManager.Instance.InvokeCalculate += calculateAction.Invoke;
    }
    public void Drop()
    {
        if (isActive == false) return;
        try
        {
            EventManager.Instance.InvokeDrop += dropAction.Invoke;
        }
        catch
        {
            Debug.LogError($"오류 발생 !", this);
        }
    }
    public void Blast()
    {
        if (isActive == false) return;
        EventManager.Instance.InvokeBlast += blastAction.Invoke;

    }

    public void Organize()
    {
        if (isActive == false) return;
        EventManager.Instance.InvokeOrganize += organizeAction.Invoke;
    }

    public void CalReset()
    {
        if (isActive == false) return;
        EventManager.Instance.InvokeCalReset += calculateAction.CalReset;
    }

    public void Focus()
    {
        if (isActive == false) return;
        // Debug.Log(focusAction.GetType(),this);
        focusAction.Invoke();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, Utils.FloatToVector2(Utils.TILE_SIZE));
    }
}
