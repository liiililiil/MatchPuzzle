using UnityEngine;

public class TwoShot : InStageManager, IInStageManager{
    protected override void OnStart()
    {
        // EventManager.Instance.OnMovedTile.AddListener(OnMoveTile);
        // EventManager.Instance.OnDisabledTile.AddListener(OnDisabledTile);
        Init(0, 2);
    }
}
