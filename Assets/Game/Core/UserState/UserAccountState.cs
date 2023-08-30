using System.Collections.Generic;
using Utils.Serialization;
using System;
using System.Linq;

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
            var boardsBytes = SerializeList(Boards.Cast<ISerializable>());
            var scenariosBytes = SerializeList(AttackScenarios.Cast<ISerializable>());
            var result = new byte[
                sizeof(int) //Version
                + sizeof(int) //Id
                + sizeof(int) + socialBytes.Length //Social lenght
                + sizeof(int) + currenciesBytes.Length //Currencies lenght
                + sizeof(byte) + boardsBytes.Count
                + sizeof(byte) + scenariosBytes.Count];

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

        private List<byte> SerializeList(IEnumerable<ISerializable> sources)
        {
            var count = sources.Count();
            var result = new List<byte>(count * 2048)
            {
                (byte)count
            };
            foreach (var source in sources)
            {
                var serializedSource = source.Serialize();
                result.AddRange(ByteConverter.Serialize(serializedSource.Length));
                result.AddRange(serializedSource);
            }
            return result;
        }

        public void Deserialize(byte[] data)
        {
            var offset = 0;

            offset += ByteConverter.ReturnFromStream(data, offset, out Version);
            offset += ByteConverter.ReturnFromStream(data, offset, out Id);

            Social = Deserialize<UserSocialState>(data, ref offset);
            Currencies = Deserialize<UserCurrenciesState>(data, ref offset);

            Boards = DeserializeList<UserBoardState>(data, ref offset);
            AttackScenarios = DeserializeList<UserAttackScenarioState>(data, ref offset);
        }

        private T Deserialize<T>(byte[] data, ref int offset) where T : ISerializable, new()
        {
            var result = new T();
            offset += ByteConverter.ReturnFromStream(data, offset, out int objectSize);
            offset += ByteConverter.ReturnFromStream(data, offset, objectSize, out var bytesToRead);
            result.Deserialize(bytesToRead);
            return result;
        }

        private List<T> DeserializeList<T>(byte[] data, ref int offset) where T : ISerializable, new()
        {
            offset += ByteConverter.ReturnFromStream(data, offset, out byte objectsCount);
            var result = new List<T>(objectsCount);
            for (int i = 0; i < objectsCount; i++)
            {
                var obj = Deserialize<T>(data, ref offset);
                result.Add(obj);
            }
            return result;
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
            };
        }
    }
}