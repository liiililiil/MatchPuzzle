using UnityEngine;

public class Blast : ITileType
{
    public bool IsDropTile()
    {
        return false;
    }

    public void Blasted(ref Tile[,] tileMap, ref Tile thisTile)
    {
        throw new System.NotImplementedException("Blast is not implemented yet.");
    }
}
