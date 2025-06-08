using UnityEngine;

public class Green : ColorTile, ITile
{
    public override TileType tileType {get => TileType.Green;}
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
