using System.Collections;
using UnityEngine;

public abstract class DropTile : Tile, ITile
{
    Coroutine movementCoroutine;
    public override sealed void Drop(){
        // //이미 움직이고 있다면 무시
        // if(movementCoroutine != null) return;
        // ITile belowTile = Raycast(Vector2.down, 1, true);

        // //밑에 타일이 없으면 밑으로
        // if(belowTile is null){
        //     movementCoroutine = StartCoroutine(movement(Vector2.down, Raycast(Vector2.up, 1, true)));
        // }
        

    

    }
}
