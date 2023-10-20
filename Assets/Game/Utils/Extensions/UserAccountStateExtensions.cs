using Core;
using System.Linq;

namespace Utils.Extensions
{
    public static class UserAccountStateExtensions
    {
        public static UserBoardState TryGetBoard(this UserAccountState state, string boardName)
        {
            return state.Boards.FirstOrDefault(b => b.Name == boardName);
        }

        public static void AddOrReplaceBoard(this UserAccountState state, UserBoardState board)
        {
            var index = state.Boards.IndexOf(board);
            if (index == -1)
                state.Boards.Add(board);
            else
                state.Boards[index] = board;
        }

        public static bool TryDeleteBoard(this UserAccountState state, string boardName)
        {
            var boardToDelete = state.TryGetBoard(boardName);
            if (boardToDelete == null)
                return false;

            state.Boards.Remove(boardToDelete);
            return true;
        }

        public static bool IsValid(this UserAccountState state) => state.Id > 0;
    }
}
