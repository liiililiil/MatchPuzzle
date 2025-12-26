using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class MenuPos
{
    public MenuState menuState;
    public Vector3 pos;
    public Vector3 scale;
    public float time;
    public EaseType easeType;
}

[Serializable]
public class MenuPosOther
{
    public Vector3 pos;
    public Vector3 scale;
    public float time;
    public EaseType easeType;
}


public class MenuPosChanger : MonoBehaviour
{    
    [SerializeField]
    private MenuPos[] menuPosArray;

    [SerializeField]
    private MenuPosOther other;

    private bool isRect = false;
    private RectTransform rectTransform = null;
    private Vector3 startPos;
    private Vector3 targetPos;

    private Vector3 startScale;
    private Vector3 targetScale;

    private float targetTime;
    private EaseType easeType;
    private float time;


    ~MenuPosChanger()
    {
        try
        {
            MenuManager.Instance.OnChangeMenuState.RemoveListener(ChangeMenuState);
        }
        catch (Exception)
        {
            // 무시
        }
    }

    private void Start() {
        if(TryGetComponent(out rectTransform))
        {
            isRect = true;
        }

        MenuManager.Instance.OnChangeMenuState.AddListener(ChangeMenuState);

        this.enabled = false;
    }

    public void ChangeMenuState(MenuState menuState)
    {
       foreach(var menuPos in menuPosArray)
        {
            if (menuPos.menuState == menuState)
            {
                SetupMenuPos(menuPos);
                return;
            }
        }

        //기타
        SetupMenuPos(other);

    }

    private void SetupMenuPos(MenuPos target)
    {
        if(isRect)
        {
            startPos = rectTransform.anchoredPosition;
            startScale = new Vector3(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y, 1);      
        }

        else
        {
            startPos = transform.position;
            startScale = transform.localScale;      
        }

        targetPos = target.pos;
        targetScale  = target.scale;
        targetTime = target.time;
        easeType = target.easeType;
        time = 0;
        this.enabled = true;
    }

    private void SetupMenuPos(MenuPosOther target)
    {
        SetupMenuPos(new MenuPos()
        {
            pos = target.pos,
            scale = target.scale,
            time = target.time,
            easeType = target.easeType
        });
    }


    private void Update() {
        time += Time.deltaTime;

        float easeT = EaseMoveMent.Ease(easeType, Mathf.Clamp01(time / targetTime));

        if (isRect)
        {
            rectTransform.anchoredPosition = Vector3.LerpUnclamped(startPos, targetPos, easeT);
            rectTransform.sizeDelta = Vector3.LerpUnclamped(startScale, targetScale, easeT);           
        }

        else
        {
            transform.position = Vector3.LerpUnclamped(startPos, targetPos, easeT);
            transform.localScale = Vector3.LerpUnclamped(startScale, targetScale, easeT);          
        }



        if (time / targetTime >= 1f)
        {
            if (isRect)
            {
                rectTransform.anchoredPosition = targetPos;
                rectTransform.sizeDelta = targetScale;

            } else{
                transform.position = targetPos;
                transform.localScale = targetScale;
            }

            this.enabled = false;
        }
        
    }

}
