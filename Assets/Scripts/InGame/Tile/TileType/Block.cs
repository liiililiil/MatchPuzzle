using UnityEngine;

public class Block : ITileType
{
    public bool IsDropTile()
    {
        return false;
    }  

    public void Blasted(ref Tile[,] tileMap, ref Tile thisTile)
    {
        thisTile.SetTileType(new Blast());
    }
}
