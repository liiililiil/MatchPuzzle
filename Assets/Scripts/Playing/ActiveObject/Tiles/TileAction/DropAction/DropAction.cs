

using UnityEngine;

// 하강 연산 기본 클래스
public class DropAction : TileAction
{
    public bool isDrop { get; set; }
    public override void Invoke()
    {

        if (!tile.isActive)
        {
            // Debug.LogWarning($"{gameObject.name}에 있는{GetType()}가 비활성 상태이므로 Invoke를 무시합니다.",this);
            return;
        }

        /// <summary>
        /// 이 코드는 임시 코드입니다. 차후 원인 발견 후 수정될 예정입니다.
        /// </summary>
        if ((Vector2)transform.position == new Vector2(Utils.WAIT_POS_X, Utils.WAIT_POS_Y))
        {
            Debug.LogWarning($"{gameObject.name}이(가) 대기 위치에 있으므로 Invoke를 무시합니다.");
            return;
        }
        


        if (tile == null)
        {
            tile = GetComponent<Tile>();

            if (tile == null)
            {
                Debug.LogError($"{gameObject.name}에게 Tile이 없습니다!");
                return;
            }

            Debug.LogWarning($"{gameObject.name}에 있는 {GetType()}가 Init되지 않았습니다. 자동으로 Init합니다.");
        }

        OnInvoke();
    }

}
