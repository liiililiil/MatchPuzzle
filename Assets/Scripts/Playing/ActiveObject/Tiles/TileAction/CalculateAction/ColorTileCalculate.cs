using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 색깔 타일 연산 액션
public class ColorTileCalculate : TileAction, ICalculateAction
{
    // 연산이 완료되었는지 여부
    private Vector2Int _isCalculated = Vector2Int.zero;

    //밖 접근용
    public Vector2Int isCalculated
    {
        get => _isCalculated;
        set => _isCalculated = value;
    }

    // 연산 초기화
    // 이거 클래스로 합쳐야하는데 귀찮음 나중에 할거임
    // 이렇게 해놓고 까먹겠지 ㅋㅋㅋ
    public void CalReset()
    {
        isCalculated = Vector2Int.zero;
        tile.isCenter = false;
        tile.length = Vector2Int.one;
    }


    protected override void OnInvoke()
    {
        // 이미 연산이 완료된 경우 무시
        if (isCalculated != Vector2Int.zero) return;

        // 연산 시작 위치 기록
        tile.isCenter = true;

        // 연산 용 스택
        Stack<Tile> totalStack = new Stack<Tile>();

        // 본인을 포함 시키기
        tile.length = Vector2Int.one;

        // 주변 타일 검사 시작
        NearbyCheck(ref tile.length, ref totalStack, Vector2Int.zero);

        //3개 이상 연결되었는지 여부
        bool needBlasted = tile.length.x >= 3 || tile.length.y >= 3;

        //스택에 쌓인 타일들 연산 완료 처리
        while (totalStack.Count > 0)
        {
            Tile tile = totalStack.Pop();

            //연산 초기화
            tile.CalReset();


            //터지는 조건이 되면 폭발
            if (needBlasted)
            {
                tile.Blast();
            }
        }
    }

    // 주변 타일을 재귀적 너비 탐색으로 검사하고 length 에 최대 길이들을 반환
    public void NearbyCheck(ref Vector2Int length, ref Stack<Tile> totalStack, Vector2Int exceptionDirection)
    {

        //본인 타일 넣기
        totalStack.Push(GetComponent<Tile>());

        //들어온 방향이 없으면 무시 없음
        if (exceptionDirection == Vector2Int.zero) {}

        // 들어온 방향의 축이 이미 연산된 경우 무시
        else if (exceptionDirection.x == 0)
        {
            if (_isCalculated.y != 0)
                return;

            // 연산 표시 및 길이 증가
            length.y++;
            _isCalculated.y++;
        }
        else if (exceptionDirection.y == 0)
        {
            if (_isCalculated.x != 0)
                return;


            // 연산 표시 및 길이 증가
            length.x++;
            _isCalculated.x++;
        }

        // 일단 들어온 방향의 축을 먼저 검사하도록 정렬
        Vector2Int reverseDirection = -exceptionDirection;
        Vector2Int[] sortedDirs = Utils.directions
            .OrderByDescending(dir => dir == reverseDirection ? 1 : 0)
            .ToArray();

        // 4방향 검사
        foreach (Vector2Int dir in sortedDirs)
        {
            // 들어온 방향은 무시
            if (dir == exceptionDirection) continue;

            // 상대 방향을 절대 방향으로 변환
            Vector2 worldDir = dir.x * (Vector2)transform.right + dir.y * (Vector2)transform.up;

            // GetTileFromWorld를 이용해 타일을 찾고 검사 수행
            ICalculateAction tile = GetTileFromWorld<ICalculateAction>(worldDir);

            // 타일이 존재하고 타입이 같으면 검사 재귀 호출
            if (tile != null && tile.IsEqualType(this.tile.tileType))
            {
                if (exceptionDirection == -dir || exceptionDirection == Vector2Int.zero)
                    tile.NearbyCheck(ref length, ref totalStack, -dir);

                //들어온 방향이 아닌 축은 길이 측정시 새로운 변수로 검사
                else
                {
                    Vector2Int temp = Vector2Int.one;
                    tile.NearbyCheck(ref temp, ref totalStack, -dir);
                    length = Vector2Int.Max(length, temp);
                }
            }
        }
    }

    // 외부 접근용 타입 비교
    public bool IsEqualType(TileType type)
    {
        return tile.tileType == type;
    }


}
