using UnityEngine;

// 스프라이트 액션 인터페이스
public interface ISpriteAction : IDestroyerAction
{
    // public Sprite sprite { get; set; }
    // 시트 길이 반환을 위한 메서드
    public int GetLenght();
}
