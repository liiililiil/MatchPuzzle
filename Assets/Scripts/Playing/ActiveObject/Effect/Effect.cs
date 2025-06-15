using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public abstract class Effect : MonoBehaviour, IEffect
{
    public abstract EffectType effectType{ get; }
    [SerializeField]
    protected Sprite[] Sheet;
    protected SpriteRenderer spriteRenderer;

    public void Active(Vector2 pos)
    {
        transform.position = pos;


        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(AnimationPlay());
    }

    public void Disable()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = null;
        transform.position = new Vector2(Utils.WAIT_POS_X, Utils.WAIT_Pos_Y);

        EffectManager.Instance.Pooling(gameObject, effectType);
    }

    protected IEnumerator AnimationPlay()
    {
        int count = Sheet.Length-1;

        while (count >= 0)
        {
            spriteRenderer.sprite = Sheet[count];

            count--;

            float time = 0f;
            while (time <= 1f)
            {
                time += Time.deltaTime * Utils.EFFECT_SPEED;
                yield return null;
            }
        }

        Disable();
    }
}
