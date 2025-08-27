using UnityEngine;

public interface IActiveObject
{
    public bool isActive { get; }
    public void Disable(bool hideEffect = false);
    public void Enable(Vector2 position, Quaternion rotate, IActiveObject startBy = null);
}
