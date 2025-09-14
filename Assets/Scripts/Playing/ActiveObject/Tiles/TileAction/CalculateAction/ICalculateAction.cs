using System.Collections.Generic;
using UnityEngine;

// 계산 액션 인터페이스
public interface ICalculateAction : ITileAction
{
    // 서브 계산시 사용
    public void NearbyCheck(ref Vector2Int length, ref Stack<Tile> totalStack, Vector2Int exceptionDirection);

    // 검사에서 사용한 변수들을 초기화
    public void CalReset();

    // 타일 타입이 같은지 검사
    public bool IsEqualType(TileType type);

    // 중복 계산 방지를 위해 검사한 좌표를 기록
    public Vector2Int isCalculated { get; set; }
}
