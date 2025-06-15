using Unity.Mathematics;
using UnityEngine;
public interface IEffect
{
    public void Active(Vector2 pos, quaternion rotate);
}
