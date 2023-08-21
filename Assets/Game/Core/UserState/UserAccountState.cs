using System.Collections.Generic;
using Utils.Serialization;
using System;

namespace Core
{
    [Serializable]
    public sealed class UserAccountState
    {
        public int Id;

        public UserSocialState Social;
        public UserCurrenciesState Currencies;
        public Dictionary<string, BoardData> Boards { get; private set; }

        public static UserAccountState GetInitial(string name)
        {
            var rnd = new Random();
            return new UserAccountState()
            {
                Id = rnd.Next(1, int.MaxValue),
                Social = new UserSocialState()
                {
                    Name = name,
                    AvatarPath = "guest/avatar1.png",
                },
                Currencies = new UserCurrenciesState()
                {
                    Crystals = 10000,
                    Gas = 0
                },
            };
        }

        public BoardData TryGetBoard(string boardName)
        {
            Boards ??= new Dictionary<string, BoardData>();
            return Boards.ContainsKey(boardName) ? Boards[boardName] : null;
        }

        public bool TryAddBoard(string boardName, BoardData board)
        {
            Boards ??= new Dictionary<string, BoardData>();
            if (Boards.ContainsKey(boardName))
                return false;

            Boards.Add(boardName, board);
            return true;
        }

        public bool TryDeleteBoard(string boardName)
        {
            Boards ??= new Dictionary<string, BoardData>();
            if (Boards.ContainsKey(boardName))
                return false;

            Boards.Remove(boardName);
            return true;
        }
    }
}