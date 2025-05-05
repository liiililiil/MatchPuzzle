using UnityEngine;

public class Blast : NotDropTileType, ITileType
{
    public void Blasted(ref Tile[,] tileMap, ref Tile thisTile, TileUtils tileUtils)
    {
        throw new System.NotImplementedException("Blast can't be Blasted.");
    }
}
