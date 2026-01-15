using UnityEngine;

public class UniversalDestroyer : MonoBehaviour
{
    // 타일과 충돌 시 타일 파괴 호출 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Tile>()?.tileType != TileType.Block)
            collision.gameObject.GetComponent<Tile>()?.Disable(true, true);
    }
}
