using UnityEngine;

public class BlastOrganize : TileAction, IOrganizeAction
{
    protected override void OnInvoke() {
        GetComponent<Tile>().Blast();
    }
}

