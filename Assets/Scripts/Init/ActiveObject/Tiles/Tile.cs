using System;
using UnityEngine;

public class Tile : MonoBehaviour, IActiveObject
{
    
    private enum TileFlag : byte
    {
        isActive = 1 << 0,
        isCenter = 1 << 1,
        isInit = 1 << 2,
        SWitched = 1 << 3
    }

    //비트플레그
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

    [SerializeField]
    private bool ingoreDropWhenSpawn;
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

    private void Start() {
        if(!isInit) Enable(transform.position, transform.rotation);
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
    // private void Start()
    // {
    //     this.Enable(transform.position, transform.rotation);
    // }

    public void hit(TileDestroyer tileDestroyer)
    {
        EventManager.Instance.OnBlastTileByBomb.Invoke(this, tileDestroyer, transform.position);
        explodedAction.Invoke();
    }

    public void Disable(bool hideEffect = false, bool notRecord = false)
    {
        
        // 리지드 바디 해제
        rigidbody2D.simulated = false;
        rigidbody2D.Sleep();

        // 하강을 위해 위 타일 검사
        Tile[] aboveTile = {
            Utils.TryGetTile(transform.position, transform.up, Utils.TILE_GAP), 
            Utils.TryGetTile(transform.position, transform.up + transform.right, Utils.TILE_GAP), 
            Utils.TryGetTile(transform.position, transform.up - transform.right, Utils.TILE_GAP) 
        };

        // 비활성화 표기
        isActive = false;

        // 콜라이더 비활성화
        GetComponent<BoxCollider2D>().enabled = false;

        // 비활성화 알림
        if(!notRecord) EventManager.Instance.OnDisabledTile.Invoke(this, transform.position);

        // 사라지는 이펙트 생성
        if(!hideEffect) SpawnManager.Instance.SpawnObject(EffectType.TileDisabled, transform.position, transform.rotation, this);

        transform.position = new Vector2(Utils.WAIT_POS_X, Utils.WAIT_POS_Y);

        // Debug.Log($"{aboveTile[0]} , {aboveTile[1]} , {aboveTile[2]}", this);
        // 하강 시도
        foreach (Tile t in aboveTile) t?.Drop();

        // 스프라이트 비활성화
        sprite.SetActive(false);

        // 방향 초기화
        transform.rotation = Quaternion.identity;

        //풀링
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
            }

            if (rigidbody2D == null)
            {
                rigidbody2D = GetComponent<Rigidbody2D>();
            }
        }

        switchedTileType = TileType.Empty;
        
        GetComponent<BoxCollider2D>().enabled = true;

        sprite.SetActive(true);

        rigidbody2D.simulated = true;

        transform.position = position;
        transform.rotation = rotate;

        EventManager.Instance.OnSpawnedActiveOjbect.Invoke(this, transform.position);

        isActive = true;

        

        if(!ingoreDropWhenSpawn) Drop();
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
        EventManager.Instance.InvokeCalculate += calculateAction.Invoke;
    }
    public void Drop()
    {
        try
        {
            EventManager.Instance.InvokeDrop += dropAction.Invoke;
        }
        catch (Exception ex)
        {
            Debug.LogError($"오류 발생 ! {ex}", this);
        }
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

    public void Focus()
    {
        focusAction.Invoke();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, Utils.FloatToVector2(Utils.TILE_SIZE));
    }
}
