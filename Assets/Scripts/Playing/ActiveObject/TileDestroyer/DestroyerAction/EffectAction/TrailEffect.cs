using System.Collections;
using UnityEngine;

public class TrailEffect : DestroyerAction, IEffectSpawnAction
{
    [SerializeField]
    private EffectType effectType;

    protected override void OnInvoke()
    {
        StartCoroutine(OnTrailEffect());

    }

    IEnumerator OnTrailEffect()
    {
        while (tileDestroyer.isActive)
        {
            if (GetEffectFromWorld() == null)
                SpawnManager.Instance.SpawnObject(effectType, transform.position, transform.rotation, tileDestroyer);
            yield return null;
        }
    }
    

    public Effect GetEffectFromWorld()
    {
        int layerMask = 1<< 13;

        Collider2D hit = Physics2D.OverlapBox(transform.position, Utils.FloatToVector2(Utils.TILE_SIZE), transform.rotation.z, layerMask);
        return hit ? hit.GetComponent<Effect>() : default;
    }
    

}
