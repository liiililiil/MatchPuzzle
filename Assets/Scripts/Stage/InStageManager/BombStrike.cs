using System;
using System.Linq;
using UnityEngine;

public class BombStrike : InStageManager, IInStageManager
{
    private int strikeLimit = 0;
    protected override void OnStart()
    {
        EventManager.Instance.OnMovedTile.AddListener(OnMoveTile);
        EventManager.Instance.OnDisabledTile.AddListener(OnDisabledTile);
        EventManager.Instance.InvokeEnterFocusPhase.AddListener(StageCheck);
        Init(0, 1);
    }
    
    private void OnDisabledTile(Tile tile, Vector2 pos)
    {
        if (TILE_CONSTANT.BOMB_TILES.Contains(tile.tileType) && strikeLimit >= 0)
        {
            strikeLimit -= 1;
            stagePrograss.leftMovement++;
        }
        
        stagePrograss.tileRecode.Record(tile.tileType);
        stagePrograss.score += 1;
    }

    private void OnMoveTile()
    {
        strikeLimit = 3;
        stagePrograss.leftMovement--;

    }
}
