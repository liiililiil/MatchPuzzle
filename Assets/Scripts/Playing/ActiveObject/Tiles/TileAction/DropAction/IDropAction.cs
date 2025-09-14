// 하강 액션 인터페이스
public interface IDropAction : ITileAction
{
    public bool isCanDrop { get; }
    public bool isDrop { get; set; }
}
