using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;



public class Tile : MonoBehaviour, IActiveObject
{
    private enum TileFlag : byte
    {
        isActive = 1 << 0,
        isCenter = 1 << 1,
        isInit = 1 << 2
    }

    private TileFlag flag;
    private ICalculateAction calculateAction;
    private IBlastAction blastAction;
    private IDropAction dropAction;
    private IOrganizeAction organizeAction;
    private IExplodedAction explodedAction;

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
    public Vector2Int length = Vector2Int.zero;

    public GameObject sprite;
    public TileType tileType;

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
            Debug.LogError($"{gameObject.name} 에게 {typeof(T)} 컴포넌트가 없습니다!");
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

    public void Disable()
    {
        Tile[] aboveTile = { GetTileFromWorld<Tile>(transform.up, true), GetTileFromWorld<Tile>(transform.up + transform.right, true), GetTileFromWorld<Tile>(transform.up - transform.right, true) };

        isActive = false;
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        if (boxCollider2D != null)
            boxCollider2D.enabled = false;
        else
            Debug.LogError($"{gameObject.name} 에게 BoxCollider2D가 없습니다!");

        EventManager.Instance.OnDisabledTile.Invoke(this, transform.position);
        SpawnManager.Instance.SpawnObject(EffectType.TileDisabled, transform.position, transform.rotation, this);

        if (dropAction.isDrop == true)
        {
            dropAction.isDrop = false;
            EventManager.Instance.movingTiles--;
        }


        transform.position = new Vector2(Utils.WAIT_POS_X, Utils.WAIT_Pos_Y);
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

            if (sprite == null)
            {
                sprite = transform.GetChild(0).gameObject;
                if (sprite == null)
                    Debug.LogError($"{gameObject.name} 에게 자식 sprite가 없습니다!");
                else
                    Debug.LogWarning($"{gameObject.name} 에게 자식 sprite가 자동으로 할당되었습니다.");
            }
            // Debug.Log("구성됨!",this);
        }

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

    public void OnplayerFocus()
    {
        StartCoroutine(OnPlayerFocusCoroutine());
    }

    IEnumerator OnPlayerFocusCoroutine()
    {
        Vector2 pos = transform.position;

        while (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float dis = -Vector2.Distance(pos, mousePos) + 1;
            sprite.transform.position = Vector2.Lerp(sprite.transform.position, mousePos * dis, 0.8f);
            yield return null;
        }

    }
    
            public T GetTileFromWorld<T>(Vector2 direction, bool isGlobal = false)
    {
        return GetTileFromWorld<T>(direction, 1, isGlobal);
    }

    public T GetTileFromWorld<T>(Vector2 direction, int revisionMultiple, bool isGlobal = false)
    {
        int layerMask = isGlobal ? ~0 : 1 << gameObject.layer;
        // 콜라이더 바깥으로 GetTileFromWorld 시작 지점 계산
        Vector2 start = (Vector2)transform.position + direction * Utils.RAYCASY_REVISION * revisionMultiple;

        // GetTileFromWorld 수행
#if UNITY_EDITOR
        DrawOverlapBox(start, Utils.FloatToVector2(Utils.TILE_SIZE), transform.rotation.z, Color.red);
#endif
        Collider2D hit = Physics2D.OverlapBox(start, Utils.FloatToVector2(Utils.TILE_SIZE), transform.rotation.z, layerMask);

        // Debug.Log(hit.collider.name
        return hit ? hit.GetComponent<T>() : default;
    }
    
    
    
#if UNITY_EDITOR
    void DrawOverlapBox(Vector2 center, Vector2 size, float angleDeg, Color color, float duration = 0.05f)
    {
        float angleRad = angleDeg * Mathf.Deg2Rad;
        Vector2 right = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        Vector2 up = new Vector2(-right.y, right.x);

        Vector2 extentsX = right * (size.x / 2f);
        Vector2 extentsY = up * (size.y / 2f);

        Vector2 topRight = center + extentsX + extentsY;
        Vector2 topLeft = center - extentsX + extentsY;
        Vector2 bottomLeft = center - extentsX - extentsY;
        Vector2 bottomRight = center + extentsX - extentsY;

        Debug.DrawLine(topLeft, topRight, color, duration);
        Debug.DrawLine(topRight, bottomRight, color, duration);
        Debug.DrawLine(bottomRight, bottomLeft, color, duration);
        Debug.DrawLine(bottomLeft, topLeft, color, duration);
    }
#endif
}
