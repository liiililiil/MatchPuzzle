using UnityEngine;

public class Tile
{
    public Chain xChain;
    public Chain yChain;
    public Chain totalChain;
    public ITileType tileType;

    public void SetTileType(ITileType newTileType)
    {
        tileType = newTileType;
    }

    

}
