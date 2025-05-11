using System.Collections.Generic;
using UnityEngine;

public interface ITile{

    public ushort xChainSelf { get; set; }
    public ushort yChainSelf { get; set; }
    public ushort totalChainSelf { get; set; }
    
    public ushort xChainTotal { get; set; }
    public ushort yChainTotal { get; set; }
    public ushort totalChainTotal { get; set; }

    public bool isCalculated { get; set; }

    public void Calculate();
    public void NearbyCheck(ref Stack<ITile> totalStack, Vector2 exceptionDirection);
    public void Blasted();
    public void ForceBlasted();
    public void Drop();

    public void Bind(SpawnManager spawnManager);

    
}
