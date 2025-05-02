using System;
using UnityEngine;

public class TileUtils : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites = new Sprite[Enum.GetValues(typeof(tileType)).Length];

    public void Blast(ref tileType tileType, Chain xChain, Chain yChain, Chain totalChain)
    {
        if((xChain.total >= 5 || yChain.total >=5) && (xChain.self == 3 || yChain.self == 3)) tileType = tileType.ColorBomb;
        // else if()
    }

    public bool IsDropTile(tileType type)
    {
        switch (type)
        {
            case tileType.Blast:
            case tileType.Block:
                return false;

            case tileType.XBomb:
            case tileType.YBomb:
            case tileType.BigBomb:
            case tileType.ColorBomb:
            case tileType.Blue:
            case tileType.Green:
            case tileType.Purple:
            case tileType.Red:
                return true;
            
            default:
                throw new System.Exception($"Invalid tile type: {type}");
        }
    }
}
