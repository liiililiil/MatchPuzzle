using UnityEngine;

public class TileMovement : MonoBehaviour
{
    private Vector2 targetPos;
    private Vector2 startPos;
    private Tile tile;
    float time = 0f;


    public void SetPosition(Vector2 startPosition, Vector2 targetPosition)
    {
        startPos = startPosition;
        targetPos = targetPosition;
        time = 0f;
        
        this.enabled = true;
    }
    
    void Start()
    {
        tile = GetComponent<Tile>();
    }

    void Update()
    {
        // 초기화
        if (time <= 0)
        {
            EventManager.Instance.dropTiles++;
        }

        tile.sprite.transform.position = Vector2.Lerp(startPos, targetPos, time);
        time += Time.deltaTime * Utils.MOVEMENT_SPEED;

        if(time >= 1f)
        {
            // 위치 보정
            tile.sprite.transform.position = targetPos;
            tile.sprite.transform.localPosition = Vector3.zero;

            tile.Drop();

            EventManager.Instance.dropTiles--;
            this.enabled = false;
        }
    }
}
