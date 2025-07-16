public interface IDropAction : ITileAction
{
    public bool isCanDrop { get; }
    public bool isDrop { get; set; }
}
