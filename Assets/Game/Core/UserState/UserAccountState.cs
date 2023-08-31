using System.Collections.Generic;
using Utils.Serialization;
using System;

namespace Core
{
    public sealed class UserAccountState : ISerializable
    {
        public int Version;
        public int Id;

        public UserSocialState Social;
        public UserCurrenciesState Currencies;
        public List<UserBoardState> Boards;
        public List<UserAttackScenarioState> AttackScenarios;

        public byte[] Serialize()
        {
            var socialBytes = Social.Serialize();
            var currenciesBytes = Currencies.Serialize();
            var boardsBytes = SerializationUtils.SerializeList(Boards);
            var scenariosBytes = SerializationUtils.SerializeList(AttackScenarios);

            var result = new byte[
                sizeof(int) //Version
                + sizeof(int) //Id
                + sizeof(int) + socialBytes.Length //Social lenght
                + sizeof(int) + currenciesBytes.Length //Currencies lenght
                + boardsBytes.Count
                + scenariosBytes.Count];

            var offset = 0;
            offset += ByteConverter.AddToStream(Version, result, offset);
            offset += ByteConverter.AddToStream(Id, result, offset);

            offset += ByteConverter.AddToStream(socialBytes.Length, result, offset);
            offset += ByteConverter.AddToStream(socialBytes, result, offset);

            offset += ByteConverter.AddToStream(currenciesBytes.Length, result, offset);
            offset += ByteConverter.AddToStream(currenciesBytes, result, offset);

            offset += ByteConverter.AddToStream(boardsBytes, result, offset);
            offset += ByteConverter.AddToStream(scenariosBytes, result, offset);

            return result;
        }

        public void Deserialize(byte[] data)
        {
            var offset = 0;

            offset += ByteConverter.ReturnFromStream(data, offset, out Version);
            offset += ByteConverter.ReturnFromStream(data, offset, out Id);

            Social = SerializationUtils.Deserialize<UserSocialState>(data, ref offset);
            Currencies = SerializationUtils.Deserialize<UserCurrenciesState>(data, ref offset);

            Boards = SerializationUtils.DeserializeList<UserBoardState>(data, ref offset);
            AttackScenarios = SerializationUtils.DeserializeList<UserAttackScenarioState>(data, ref offset);
        }

        public static UserAccountState GetInitial(string name)
        {
            var rnd = new Random();
            return new UserAccountState()
            {
                Id = rnd.Next(1, int.MaxValue),
                Social = new UserSocialState()
                {
                    FacebookId = "",
                    Name = name,
                    AvatarPath = "guest/avatar1.png",
                },
                Currencies = new UserCurrenciesState()
                {
                    Crystals = 1000,
                    Gas = 250
                },
                Boards = new List<UserBoardState>(),
                AttackScenarios = new List<UserAttackScenarioState>()
                {
                    UserAttackScenarioState.GetInitial("ïnitial")
                }
            };
        }
    }
}