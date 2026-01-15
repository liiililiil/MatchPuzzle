using UnityEngine;

public class DebugStageInGameManager : InStageManager, IInStageManager
{
    protected override void OnStart()
    {
        EventManager.Instance.OnDisabledTile.AddListener(OnDisabledTile);
        Init(0, 5);
    }


    private void OnDisabledTile(Tile tile, Vector2 pos)
    {
        stagePrograss.tileRecode.Record(tile.tileType);
    }


}
