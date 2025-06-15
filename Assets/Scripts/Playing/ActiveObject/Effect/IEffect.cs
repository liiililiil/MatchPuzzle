using Unity.Mathematics;
using UnityEngine;
public interface IEffect
{
    public EffectType effectType { get; }
    public void Active(Vector2 pos);
}
