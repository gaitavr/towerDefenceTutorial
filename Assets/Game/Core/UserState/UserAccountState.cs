using System.Collections.Generic;
using Utils.Serialization;
using System;

namespace Core
{
    public sealed class UserAccountState : ISerializable
    {
        public short Version;
        public int Id;
        public UserSocialState Social;
        public UserCurrenciesState Currencies;
        public List<UserBoardState> Boards;
        public UserAttackScenarioState AttackScenario;
        
        public short GetLenght()
        {
            var lenght = sizeof(short) 
                + sizeof(int) 
                + Social.GetLenght()
                + Currencies.GetLenght()
                + SerializationUtils.GetSizeOfList(Boards)
                + AttackScenario.GetLenght();
            return (short)lenght;
        }

        public void Serialize(byte[] data, ref int offset)
        {
            offset += ByteConverter.AddToStream(Version, data, offset);
            offset += ByteConverter.AddToStream(Id, data, offset);

            Social.Serialize(data, ref offset);
            
            Currencies.Serialize(data, ref offset);

            SerializationUtils.SerializeList(Boards, data, ref offset);
            
            AttackScenario.Serialize(data, ref offset);
        }

        public void Deserialize(byte[] data, ref int offset)
        {
            offset += ByteConverter.ReturnFromStream(data, offset, out Version);
            offset += ByteConverter.ReturnFromStream(data, offset, out Id);

            Social = SerializationUtils.Deserialize<UserSocialState>(data, ref offset);
            Currencies = SerializationUtils.Deserialize<UserCurrenciesState>(data, ref offset);
            
            Boards = SerializationUtils.DeserializeList<UserBoardState>(data, ref offset);
            AttackScenario = SerializationUtils.Deserialize<UserAttackScenarioState>(data, ref offset);
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
                    Gas = 250,
                    Energy = 100
                },
                Boards = new List<UserBoardState>(),
                AttackScenario = UserAttackScenarioState.GetInitial()
            };
        }
    }
}