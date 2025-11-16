using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : Managers<SceneLoadManager>
{
    public float progress {get; private set;}
    public bool isLoaded {get; private set;}

    private static Coroutine coroutine = null;
    protected override void OnAwake() {
        SceneLoad("Menu");
    }
    public void SceneLoad(string scene)
    {

        if(coroutine != null){
            Debug.LogError("씬이 이미 로드중입니다!");
            return;
        }

        isLoaded = false;
        coroutine = StartCoroutine(SceneLoadAsync(scene));
    }

    private IEnumerator SceneLoadAsync(string name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        operation.allowSceneActivation = false;

        while (operation.isDone)
        {
            Debug.Log(operation.progress);
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

        CoroutineStop(coroutine);
    }

    private void CoroutineStop(Coroutine coroutine)
    {
        if(coroutine == null) return;
        StopCoroutine(coroutine);
        coroutine = null;
    }
}

