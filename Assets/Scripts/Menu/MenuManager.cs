using System;
using System.Collections;
using UnityEngine;


[Serializable]
public enum MenuState{
    Main,
    Setting,
    Warning,
    Select,
    MiniGame,
    inGame
}


public class MenuManager : Managers<MenuManager>
{
    public SimpleEvent<MenuState> OnChangeMenuState = new SimpleEvent<MenuState>();
    private void Start()
    {
        StartCoroutine(delayedInvoke());
    }

    IEnumerator delayedInvoke()
    {
        yield return new WaitForSeconds(1);
        // Debug.Log("MenuManager Start Invoke Main");
        OnChangeMenuState.Invoke(MenuState.Main);
    }

    ~MenuManager()
    {
        try
        {
            Instance = null;
        }
        catch (Exception)
        {
            // 무시
        }
        
    }

    public void ChangeMenu(MenuState menuState)
    {
        OnChangeMenuState.Invoke(menuState);
    }

}
