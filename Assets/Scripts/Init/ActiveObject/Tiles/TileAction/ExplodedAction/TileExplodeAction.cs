// 피 폭발 기본 클래스
using UnityEngine;

public class TileExplodeAction : TileAction
{
    public override void Invoke()
    {
        //카메라 밖에 있으면 무시
        Vector3 viewPos = Camera.main.WorldToViewportPoint(gameObject.transform.position);

        if (viewPos.z < 0 || viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
        {
            Debug.LogWarning($"{gameObject.name}이 카메라 밖에 있어 피 폭발 액션을 무시합니다.");
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

        if (!tile.isActive)
        {
            // Debug.LogWarning($"{gameObject.name}에 있는{GetType()}가 비활성 상태이므로 Invoke를 무시합니다.");
            return;
        }

        

        OnInvoke();
    }

}
