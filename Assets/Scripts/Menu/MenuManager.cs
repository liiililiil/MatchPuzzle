using UnityEngine;


public enum MenuState{
    Main,
    Setting,
    Warning,
    Select
}


public class MenuManager : MonoBehaviour
{
    SimpleEvent<MenuState> OnChangeMenuState = new SimpleEvent<MenuState>();

    public void ChangeMenu(MenuState menuState)
    {
        OnChangeMenuState.Invoke(menuState);
    }
}
