
// 드랍 불가 액션
public class NoDrop : DropAction, IDropAction
{
    public bool isCanDrop { get { return false; } }
    protected override void OnInvoke()
    {
    }
}
