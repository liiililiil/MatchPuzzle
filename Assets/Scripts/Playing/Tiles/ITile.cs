using System.Collections.Generic;
using UnityEngine;

public interface ITile{
    public Chain xChain {get; set;}
    public Chain yChain {get; set;}
    public Chain totalChain { get; set; }

    public bool isCalculated { get; set; }


    public void Calculate();
    public void NearbyCheck(ref Stack<ITile> totalStack, Vector2 exceptionDirection);
    public void Blasted();
    public void ForceBlasted();
    public void Organize();
    public void CalReset();

    
}
