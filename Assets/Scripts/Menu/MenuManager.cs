using System;
using System.Collections;
using UnityEngine;


[Serializable]
public enum MenuState{
    Main = 0,
    Setting = 1,
    Warning = 2,
    Select = 3,
    MiniGame = 4,
    InGame = 5,
    End = 6,
}


public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }
    
    public SimpleEvent<MenuState> OnChangeMenuState = new SimpleEvent<MenuState>();

    public MenuState currentMenu = MenuState.Main;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {

        // StartCoroutine(delayedInvoke());
        OnChangeMenuState.AddListener(MenuChangeHandler);
    }

    // IEnumerator delayedInvoke()
    // {
    //     yield return new WaitForSeconds(0.5f);
    //     // Debug.Log("MenuManager Start Invoke Main");
    //     OnChangeMenuState.Invoke(MenuState.Main);
    // }

    private void MenuChangeHandler(MenuState menuState)
    {
        currentMenu = menuState;

    }

    public void ChangeMenu(int menuState)
    {
        OnChangeMenuState.Invoke((MenuState)menuState);
        // Debug.Log("Menu Changed to " + (MenuState)menuState);
    }



}
