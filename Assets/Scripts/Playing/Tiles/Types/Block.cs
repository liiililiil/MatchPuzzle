using System.Collections.Generic;
using UnityEngine;

public class Block : NonDropTile, ITile
{
    public override void Calculate()
    {
        // 블록은 계산하지 않음
        isCalculated = true;
    }
    public override void NearbyCheck(ref Stack<ITile> totalStack, Vector2 exceptionDirection)
    {
        return;
    }

    public override void Blasted()
    {
        // 블록은 폭발하지 않음
        return;
    }

    public override void Organize()
    {
        // 블록은 폭발하지 않음
        return;
    }
}


