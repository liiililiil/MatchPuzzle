using UnityEngine;

public interface IActiveObject
{
    public bool isActive { get; set; }
    public void Disable();
    public void Enable(Vector2 position, Quaternion rotate, IActiveObject startBy = null);
}
