using System;
using UnityEngine;

public class TileUtils : MonoBehaviour{
    public void SetTileType(ITileType newTileType, ref Tile thisTile){
        thisTile.tileType = newTileType;
    }
    public ITileType Blast = new Blast();
    public ITileType Block = new Block();
    public ITileType Box = new Box();
    public ITileType Blue = new Blue(); 
    public ITileType Green = new Green();
    public ITileType Purple = new Purple();
    public ITileType Red = new Red();
    public ITileType XBomb = new XBomb();
    public ITileType YBomb = new YBomb();
}