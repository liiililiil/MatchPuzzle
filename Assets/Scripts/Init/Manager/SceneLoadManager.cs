using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : Managers<SceneLoadManager>
{
    public float progress {get; private set;}
    public bool isLoaded {get; private set;}

    private byte phaseStopCaller;

    private static Coroutine coroutine = null;


    public SimpleEvent OnStartLoading = new SimpleEvent();
    public SimpleEvent OnCompleteLoading = new SimpleEvent();
    protected void Start() {
        SceneLoad("BombStrikeStage");
    }
    public void SceneLoad(string scene, Action callback = null)
    {

        if(coroutine != null){
            Debug.LogError("씬이 이미 로드중입니다!");
            return;
        }

        //버그 방지로 게임 중지시키기
        EventManager.Instance.ForcePause(out phaseStopCaller);

        isLoaded = false;
        // 로딩 시작
        OnStartLoading.Invoke();

        coroutine = StartCoroutine(SceneLoadAsync(scene, callback));
    }

    private IEnumerator SceneLoadAsync(string name, Action callback)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            if(operation.progress < 0.9f)
            {
                progress = operation.progress;
            }
            else
            {
                progress = 1;
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        isLoaded = true;

        
        // Debug.Log("완료");

        //콜백
        callback?.Invoke();

        //완료 처리
        OnCompleteLoading.Invoke();

        //다시 흐르게 하기
        EventManager.Instance.ForceRelease(phaseStopCaller, Phase.Drop);


        CoroutineStop(ref coroutine);
    }

    private void CoroutineStop(ref Coroutine coroutine)
    {
        if(coroutine == null) return;
        StopCoroutine(coroutine);
        coroutine = null;
    }
}

