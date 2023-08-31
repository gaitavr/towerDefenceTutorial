using GamePlay.Attack;
using System.Collections.Generic;
using System.Text;
using Utils.Serialization;

namespace Core
{
    public sealed class UserAttackScenarioState : ISerializable
    {
        public int Version;
        public string Name;
        public List<Wave> Waves;

        public byte[] Serialize()
        {
            var nameBytes = Encoding.UTF8.GetBytes(Name);
            var wavesBytes = SerializationUtils.SerializeList(Waves);

            var result = new byte[
                sizeof(int) 
                + sizeof(byte) + nameBytes.Length
                + wavesBytes.Count];

            var offset = 0;
            offset += ByteConverter.AddToStream(Version, result, offset);
            offset += ByteConverter.AddToStream((byte)nameBytes.Length, result, offset);
            offset += ByteConverter.AddToStream(nameBytes, result, offset);
            offset += ByteConverter.AddToStream(wavesBytes, result, offset);

            return result;
        }

        public void Deserialize(byte[] data)
        {
            var offset = 0;

            offset += ByteConverter.ReturnFromStream(data, offset, out Version);
            Name = SerializationUtils.DeserealizeString(data, ref offset);
            Waves = SerializationUtils.DeserializeList<Wave>(data, ref offset);
        }

        public static UserAttackScenarioState GetInitial(string name)
        {
            return new UserAttackScenarioState()
            {
                Version = 1,
                Name = name,
                Waves = new List<Wave>()
                {
                    new Wave()
                    {
                        Version = 1,
                        Sequences = new List<SpawnSequence>()
                        {
                            new SpawnSequence()
                            {
                                Version = 1,//TODO think about it
                                EnemyType = EnemyType.Chomper,
                                Count = 100,
                                Cooldown = 0.25f
                            },
                            new SpawnSequence()
                            {
                                Version = 1,
                                EnemyType = EnemyType.Golem,
                                Count = 5,
                                Cooldown = 1.5f
                            },
                        }
                    }
                }
            };
        }

        public sealed class Wave : ISerializable
        {
            public int Version;
            public List<SpawnSequence> Sequences;

            public byte[] Serialize()
            {
                var sequencesBytes = SerializationUtils.SerializeList(Sequences);

                var result = new byte[
                    sizeof(int)
                    + sequencesBytes.Count];

                var offset = 0;
                offset += ByteConverter.AddToStream(Version, result, offset);
                offset += ByteConverter.AddToStream(sequencesBytes, result, offset);

                return result;
            }

            public void Deserialize(byte[] data)
            {
                var offset = 0;

                offset += ByteConverter.ReturnFromStream(data, offset, out Version);
                Sequences = SerializationUtils.DeserializeList<SpawnSequence>(data, ref offset);
            }
        }

        public sealed class SpawnSequence : ISerializable
        {
            public int Version;
            public EnemyType EnemyType;
            public int Count;
            public float Cooldown;

            public byte[] Serialize()
            {
                var result = new byte[
                    sizeof(int)
                    + sizeof(byte)
                    + sizeof(int)
                    + sizeof(float)];

                var offset = 0;
                offset += ByteConverter.AddToStream(Version, result, offset);
                offset += ByteConverter.AddToStream((byte)EnemyType, result, offset);
                offset += ByteConverter.AddToStream(Count, result, offset);
                offset += ByteConverter.AddToStream(Cooldown, result, offset);

                return result;
            }

            public void Deserialize(byte[] data)
            {
                var offset = 0;

                offset += ByteConverter.ReturnFromStream(data, offset, out Version);
                offset += ByteConverter.ReturnFromStream(data, offset, out byte enemyType);
                EnemyType = (EnemyType)enemyType;
                offset += ByteConverter.ReturnFromStream(data, offset, out Count);
                offset += ByteConverter.ReturnFromStream(data, offset, out Cooldown);
            }
        }
    }
}
