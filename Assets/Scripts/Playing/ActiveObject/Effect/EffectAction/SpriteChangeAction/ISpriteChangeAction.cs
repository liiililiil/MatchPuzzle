using UnityEngine;
using UnityEngine.Events;

public interface ISpriteChangeAction : IEffectAction
{
    public int spriteCount { get; set; }
}
