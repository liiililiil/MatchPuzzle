using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class MenuPos
{
    public MenuState menuState;
    public Vector3 pos;
    public float time;
    public EaseType easeType;
}


public class MenuPosChanger : MonoBehaviour
{    
    [SerializeField]
    private MenuPos[] menuPosArray;


    private Vector3 startPos;
    private Vector3 targetPos;
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
        
        MenuManager.Instance.OnChangeMenuState.AddListener(ChangeMenuState);

        this.enabled = false;
    }

    public void ChangeMenuState(MenuState menuState)
    {
       
        for (int i = 0; i < menuPosArray.Length; i++)
        {
            if (menuPosArray[i].menuState == menuState)
            {
                startPos = transform.position;
                targetPos = menuPosArray[i].pos;
                targetTime = menuPosArray[i].time;
                easeType = menuPosArray[i].easeType;
                time = 0;
                this.enabled = true;
                return;
            }
        }

       
    }

    private void Update() {
        time += Time.deltaTime;

        float easeT = EaseMoveMent.Ease(easeType, Mathf.Clamp01(time / targetTime));
        transform.position = Vector3.LerpUnclamped(startPos, targetPos, easeT);

        if (time / targetTime >= 1f)
        {
            transform.position = targetPos;
            this.enabled = false;
        }
        
    }

}
