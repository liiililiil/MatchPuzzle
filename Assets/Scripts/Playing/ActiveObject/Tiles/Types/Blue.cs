using UnityEngine;

public class Blue : ColorTile, ITile
{
    public override TileType tileType {get => TileType.Blue;}
    public bool cal = false;
    public bool drop = false;

    void Update()
    {
        if (cal) Calculate();

        if (drop)
        {
            drop = false;
            Drop();
        }

    }
}
