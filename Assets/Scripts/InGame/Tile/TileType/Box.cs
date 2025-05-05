using Unity.VisualScripting;
using UnityEngine;

public class Box : DropTIleType, ITileType
{
    
    public void Blasted(ref Tile[,] tiles, ref Tile thisTile, TileUtils tileUtils){
        tileUtils.SetTileType(tileUtils.Blast, ref thisTile);
    }
}
