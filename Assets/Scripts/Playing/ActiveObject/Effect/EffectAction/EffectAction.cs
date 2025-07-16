using UnityEngine;

public class EffectAction : MonoBehaviour
{
    protected Effect effect = null;
    public void Init(Effect effect)
    {
        this.effect= effect;
        // Debug.Log("초기화 성공!");
    }

    public virtual void Invoke()
    {
        if (effect == null)
        {
            effect = GetComponent<Effect>();

            if (effect == null)
            {
                Debug.LogError($"{gameObject.name}에게 Effect가 없습니다!");
                return;
            }

            Debug.LogWarning($"{gameObject.name}에 있는 {GetType()}가 Init되지 않았습니다. 자동으로 Init합니다.");
        }

        if (!effect.isActive)
        {
            Debug.LogWarning($"{gameObject.name}에 있는{GetType()}가 비활성 상태이므로 Invoke를 무시합니다.");
            return;
        }

        OnInvoke();
    }

    protected virtual void OnInvoke()
    {
        Debug.LogWarning($"{gameObject.name}에 있는 {GetType()}가 Invoke되었지만 구성되지 않아 무시되었습니다.");
    }
}
