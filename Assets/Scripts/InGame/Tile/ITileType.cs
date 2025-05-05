public interface ITileType
{
    public bool IsDropTile();
    public void Blasted(ref Tile[,] tileMap,ref Tile thisTile, TileUtils tileUtils);    
}
