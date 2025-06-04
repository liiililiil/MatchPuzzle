using Unity.Mathematics;
using UnityEngine;

public interface ITileDestroyer
{
    public ITile startBy { get; set; }
    public void Fire(ITile startBy, Vector2 pos, quaternion rotate);
}
