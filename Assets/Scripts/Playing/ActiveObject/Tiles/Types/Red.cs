using UnityEngine;

public class Red : ColorTile, ITile
{
    public override TileType tileType {get => TileType.Red;}
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
