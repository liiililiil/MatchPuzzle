using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    public OneTimeAction InvokeCalReset = new OneTimeAction();
    public OneTimeAction InvokeCalculate = new OneTimeAction();
    public OneTimeAction InvokeDrop = new OneTimeAction();
    public OneTimeAction InvokeBlast = new OneTimeAction();
    public OneTimeAction InvokeOrganize = new OneTimeAction();

    //포커싱 후 폭발할때 피 포커싱 타일은 바로 해제하기 위한 액션
    public OneTimeAction InvokeFocusBlast = new OneTimeAction();
    public OneTimeAction InvokeReMove = new OneTimeAction();

    //타일 전체에 대한 이벤트트
    [HideInInspector]
    public UnityEvent InvokeSpawnTile;
    [HideInInspector]
    public UnityEvent<Tile, Vector2> OnDisabledTile;
    [HideInInspector]
    public UnityEvent<Tile, Vector2> OnBlastTile;
    [HideInInspector]
    public UnityEvent<Tile, TileDestroyer, Vector2> OnBlastTileByBomb;
    [HideInInspector]
    public UnityEvent<Tile, Vector2> OnBombActived;
    [HideInInspector]
    public UnityEvent<Tile, Vector2> OnOrganized;
    [HideInInspector]
    public UnityEvent<IActiveObject, Vector2> OnSpawnedActiveOjbect;

    public int movingTiles;
    public int dropTiles;
    public int moveTestTiles;
    public bool readyToFocus;
    public bool NeedTestCalculation;
    public int activeDestroyer;
    



    void Awake()
    {
        //싱글톤
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // OnDisabledTile.AddListener(deBugTest);
    }
    private void deBugTest(Tile tile, Vector2 vector2)
    {
        // SpawnManager.Instance.SpawnObject(DestroyerType.Straight, vector2, Quaternion.Euler(0, 0, 1), tile);
    }


    public void MoveTest()
    {
        StartCoroutine(MoveTestWait());
    }

    IEnumerator MoveTestWait()
    {
        while(moveTestTiles > 0)
        {
            yield return null;
        }

        // Debug.Log("테스트 시작");

        InvokeCalReset.Invoke();
        InvokeCalculate.Invoke();

        // 폭발이 필요 없는 경우 되돌리기
        if (InvokeBlast.Count <= 0)
        {
            // Debug.Log("폭발 필요 없음");
            InvokeFocusBlast.Clear();
            InvokeReMove.Invoke();
        }
        else
        {
            // Debug.Log("폭발 필요");
            InvokeReMove.Clear();
            InvokeFocusBlast.Invoke();
            readyToFocus = false;
        }
    }


    //생명 주기

    private void LateUpdate()
    {
        //포커싱 될 준비가 끝나면 포커싱 대기
        if (readyToFocus == true)
        {
            return;
        }

        InvokeBlast.Invoke();

        if (activeDestroyer <= 0 && dropTiles <= 0)
        {

            InvokeSpawnTile.Invoke();
            
            if (dropTiles <= 0)
            {
                InvokeDrop.Invoke();
            }


            if (movingTiles <= 0)
            {
                InvokeCalReset.Invoke();
                InvokeCalculate.Invoke();

                if (InvokeBlast.Count <= 0)
                {
                    readyToFocus = true;
                }
            }


        }
    }
    

}
