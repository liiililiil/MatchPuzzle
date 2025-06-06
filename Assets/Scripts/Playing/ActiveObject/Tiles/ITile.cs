using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public interface ITile
{
    public Chain xChain { get; set; }
    public Chain yChain { get; set; }
    public Chain totalChain { get; set; }
    public TileType tileType { get; }
    public bool isCanDrop { get; }

    public bool isCalculated { get; set; }
    public bool isDrop { get; set; }
    


    public void Calculate();
    public void NearbyCheck(ref Stack<ITile> totalStack, Vector2 exceptionDirection);
    public void Blasted();
    public void ForceBlasted();
    public void Organize();
    public void CalReset();
    public void Drop();
    public void Enable(Vector2 pos, quaternion rotate);
    public bool isCoroutineRunning();


}
