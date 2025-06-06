using UnityEngine;

public class Purple : ColorTile, ITile
{
    public override TileType tileType {get => TileType.Purple;}
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
