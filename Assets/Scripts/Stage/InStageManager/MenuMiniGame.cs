using UnityEngine;

public class MenuMiniGame : InStageManager, IInStageManager
{
    private int count = 0;
    protected override void OnStart()
    {
        EventManager.Instance.OnMovedTile.AddListener(OnMoveTile);
        EventManager.Instance.OnDisabledTile.AddListener(OnDisabledTile);
        Init(0, -1);
    }

    private void OnDisabledTile(Tile tile, Vector2 pos)
    {
        stagePrograss.tileRecode.Record(tile.tileType);
        stagePrograss.score += 10;
    }

    private void OnMoveTile()
    {
        if(MenuManager.Instance.currentMenu != MenuState.Main) return;

        count += 1;

        if(count > 5)
        {
            count = 0;
            MenuManager.Instance.ChangeMenu((int)MenuState.MiniGame);
        }
    }

    ~MenuMiniGame()
    {
        try
        {
            EventManager.Instance.OnMovedTile.RemoveListener(OnMoveTile);
            EventManager.Instance.OnDisabledTile.RemoveListener(OnDisabledTile);
        }
        catch (System.Exception)
        {
            // 무시
        }
    }


}
