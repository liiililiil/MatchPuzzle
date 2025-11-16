using System.Collections;
using UnityEngine;

// 파괴 효과를 일정 간격으로 생성하는 액션
public class TrailEffect : DestroyerAction, IEffectSpawnAction
{
    [SerializeField]
    private EffectType effectType;

    protected override void OnInvoke()
    {
        StartCoroutine(OnTrailEffect());

    }

    // 파괴자가 활성화 중일 때 일정 간격으로 효과 생성
    IEnumerator OnTrailEffect()
    {
        while (tileDestroyer.isActive)
        {
            // 현재 위치에 이미 효과가 없을 때만 생성
            if (GetEffectFromWorld() == null)
                SpawnManager.Instance.SpawnObject(effectType, transform.position, transform.rotation, tileDestroyer);
                
            yield return null;
        }
    }


    // 현재 위치에 이미 효과가 있는지 확인하고, 있으면 반환
    public Effect GetEffectFromWorld()
    {
        int layerMask = 1 << 13;

        Collider2D hit = Physics2D.OverlapBox(transform.position, Utils.FloatToVector2(Utils.TILE_SIZE), transform.rotation.z, layerMask);
        return hit ? hit.GetComponent<Effect>() : default;
    }


}
