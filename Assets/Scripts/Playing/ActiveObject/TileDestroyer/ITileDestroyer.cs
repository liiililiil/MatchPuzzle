using Unity.Mathematics;
using UnityEngine;

public interface ITileDestroyer
{
    public Tile startBy { get; set; }
    public void Fire(Tile startBy, Vector2 pos, quaternion rotate);
}
