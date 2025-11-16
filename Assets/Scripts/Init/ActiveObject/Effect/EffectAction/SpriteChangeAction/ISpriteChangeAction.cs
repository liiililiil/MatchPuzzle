using UnityEngine;
using UnityEngine.Events;

// 스프라이트 변경을 위한 액션
public interface ISpriteChangeAction : IEffectAction
{
    // 재생될 스프라이트 개수
    public byte spriteCount { get; set; }
}
