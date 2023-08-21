using System.Collections.Generic;
using Utils.Serialization;
using System;

namespace Core
{
    [Serializable]
    public sealed class UserAccountState
    {
        public string Id;

        public UserSocialState Social;
        public UserCurrenciesState Currencies;
        public Dictionary<string, BoardData> Boards;

        public static UserAccountState GetInitial(string name)
        {
            var rnd = new Random();
            return new UserAccountState()
            {
                Id = rnd.Next(1, int.MaxValue).ToString(),
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
                Boards = new Dictionary<string, BoardData>()
            };
        }
    }
}