using System.Collections.Generic;
using UnityEngine;

public interface ITile{

    public ushort xChainSelf { get; set; }
    public ushort yChainSelf { get; set; }
    public ushort totalChainSelf { get; set; }
    
    public ushort xChainTotal { get; set; }
    public ushort yChainTotal { get; set; }
    public ushort totalChainTotal { get; set; }

    public void StartNearbyCheck();
    public void NearbyCheck(ref HashSet<ITile> totalStack, Vector2 exceptionDirection);

    public void RaycastNearby(ref HashSet<ITile> totalStack, Vector2 exceptionDirection);
    public ITile Raycast(Vector2 direction);
}
