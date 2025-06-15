using Unity.Mathematics;
using UnityEngine;

public abstract class Effect : MonoBehaviour, IEffect
{
    [SerializeField]
    protected Sprite[] Sheet;

    public abstract void Active(Vector2 pos, quaternion rotate);

    public void Disable()
    {
        
    }
}
