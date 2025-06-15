using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class DisabledEffect : Effect, IEffect
{
    public override EffectType effectType { get => EffectType.Disabled; }
}
