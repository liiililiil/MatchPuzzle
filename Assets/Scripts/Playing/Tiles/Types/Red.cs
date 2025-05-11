using UnityEngine;

public class Red : ColorTile, ITile
{
    public bool cal = false;

    void Update()
    {
        if(cal) Calculate();
    }

}
