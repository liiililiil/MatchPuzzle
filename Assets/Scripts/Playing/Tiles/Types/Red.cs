using UnityEngine;

public class Red : ColorTile, ITile
{
    public bool cal = false;
    public bool drop = false;
    public override sealed TileType tileType {get => TileType.Red;} 

    void Update()
    {
        if(cal) Calculate();
        if(drop){
            drop =false;
            Drop();
        }

    }

}
