using UnityEngine;

public class DeBug : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (new Vector3(1, 0) * Utils.TILE_GAP));
        Gizmos.DrawLine(transform.position, transform.position + (new Vector3(-1, 0) * Utils.TILE_GAP));
        Gizmos.DrawLine(transform.position, transform.position + (new Vector3(0, 1) * Utils.TILE_GAP));
        Gizmos.DrawLine(transform.position, transform.position + (new Vector3(0, -1) * Utils.TILE_GAP));
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
