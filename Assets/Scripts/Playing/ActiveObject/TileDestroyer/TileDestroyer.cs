using Unity.Mathematics;
using UnityEngine;

public abstract class TileDestroyer : MonoBehaviour, ITileDestroyer
{
    public Tile startBy { get; set; }
    public void Fire(Tile startBy, Vector2 pos, quaternion rotate)
    {
        this.startBy = startBy;
        transform.position = pos;
        transform.rotation = rotate;
    }
}
