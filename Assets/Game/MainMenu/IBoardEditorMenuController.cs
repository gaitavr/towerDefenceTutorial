
namespace MainMenu
{
    public interface IBoardEditorMenuController
    {
        void Select(BoardsEditorMenuItem item);
        void Delete(BoardsEditorMenuItem item);
        void Rename(BoardsEditorMenuItem item, string newName);
    }
}
