using System.Collections.Generic;
using UnityEngine;

public interface ITile{
    public Chain xChain {get; set;}
    public Chain yChain {get; set;}
    public Chain totalChain { get; set; }

    public bool isCalculated { get; set; }

    public TileType tileType {get;}


    public void Calculate();
    public void NearbyCheck(ref Stack<ITile> totalStack, Vector2 exceptionDirection);
    public void Blasted();
    public void ForceBlasted();
    public void Drop();
    public void Organize();

    public void Bind(SpawnManager spawnManager);

    
}
