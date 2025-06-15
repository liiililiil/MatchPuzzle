using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance { get; private set; }

    [SerializeField]
    private EffectData[] effectData;
    private EffectData[] EffectDataIndex = new EffectData[Utils.EFFECTTYPE_LENGHT];

    void Awake()
    {

        //싱글톤
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //인덱싱
        foreach (var data in effectData)
        {
            ushort type = (ushort)data.effectType;

            // Debug.Log(data.tileType);

            if (EffectDataIndex[type] == null) EffectDataIndex[type] = data;
        }

        //이벤트에 등록
        EventManager.Instance.OnDisabledTile.AddListener(SpawnDisabledEffect);
    }
    public void SpawnDisabledEffect(ITile tile, Vector2 pos)
    {
        GameObject effect = GetEffect(EffectType.Disabled);
        effect.transform.position = pos;
        effect.GetComponent<IEffect>().Active(pos);

    }

    public void Pooling(GameObject gameObject, EffectType effectType)
    {
        EffectDataIndex[(int)effectType].pooling.Enqueue(gameObject);
    }

    public GameObject GetEffect(EffectType effectType)
    {
        if (EffectDataIndex[(int)effectType].pooling.Count > 0) return EffectDataIndex[(int)effectType].pooling.Dequeue();
        return Instantiate(EffectDataIndex[(int)effectType].prefab);
    }
}
