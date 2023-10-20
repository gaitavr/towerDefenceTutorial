using GamePlay.Attack;
using System;
using System.Collections.Generic;
using Utils.Serialization;

namespace Core
{
    public sealed class UserAttackScenarioState : ISerializable
    {
        public short Version;
        public DateTime CreationDate;
        public List<Wave> Waves;

        public short GetLenght()
        {
            var lenght = sizeof(short) + sizeof(long) + SerializationUtils.GetSizeOfList(Waves);
            return (short)lenght;
        }

        public void Serialize(byte[] data, ref int offset)
        {
            offset += ByteConverter.AddToStream(Version, data, offset);
            offset += ByteConverter.AddToStream(CreationDate.Ticks, data, offset);
            SerializationUtils.SerializeList(Waves, data,ref  offset);
        }

        public void Deserialize(byte[] data, ref int offset)
        {
            offset += ByteConverter.ReturnFromStream(data, offset, out Version);
            offset += ByteConverter.ReturnFromStream(data, offset, out long ticks);
            CreationDate = new DateTime(ticks);
            Waves = SerializationUtils.DeserializeList<Wave>(data, ref offset);
        }

        public static UserAttackScenarioState GetInitial()
        {
            return new UserAttackScenarioState()
            {
                Version = 1,
                CreationDate = DateTime.UtcNow,
                Waves = new List<Wave>()
                {
                    new Wave()
                    {
                        Version = 1,
                        Sequences = new List<SpawnSequence>()
                        {
                            new SpawnSequence()
                            {
                                EnemyType = EnemyType.Chomper,
                                Count = 10,
                                Cooldown = 0.25f
                            },
                            new SpawnSequence()
                            {
                                EnemyType = EnemyType.Golem,
                                Count = 5,
                                Cooldown = 1.0f
                            },
                            new SpawnSequence()
                            {
                                EnemyType = EnemyType.Elien,
                                Count = 3,
                                Cooldown = 0.75f
                            },
                            new SpawnSequence()
                            {
                                EnemyType = EnemyType.Grenadier,
                                Count = 1,
                                Cooldown = 5f
                            },
                        }
                    },
                    new Wave()
                    {
                        Version = 1,
                        Sequences = new List<SpawnSequence>()
                        {
                            new SpawnSequence()
                            {
                                EnemyType = EnemyType.Chomper,
                                Count = 30,
                                Cooldown = 0.25f
                            },
                            new SpawnSequence()
                            {
                                EnemyType = EnemyType.Golem,
                                Count = 15,
                                Cooldown = 1.0f
                            },
                            new SpawnSequence()
                            {
                                EnemyType = EnemyType.Elien,
                                Count = 9,
                                Cooldown = 0.75f
                            },
                            new SpawnSequence()
                            {
                                EnemyType = EnemyType.Grenadier,
                                Count = 3,
                                Cooldown = 5f
                            },
                        }
                    },
                    new Wave()
                    {
                        Version = 1,
                        Sequences = new List<SpawnSequence>()
                        {
                            new SpawnSequence()
                            {
                                EnemyType = EnemyType.Chomper,
                                Count = 100,
                                Cooldown = 0.25f
                            },
                            new SpawnSequence()
                            {
                                EnemyType = EnemyType.Golem,
                                Count = 25,
                                Cooldown = 1.0f
                            },
                            new SpawnSequence()
                            {
                                EnemyType = EnemyType.Elien,
                                Count = 15,
                                Cooldown = 0.75f
                            },
                            new SpawnSequence()
                            {
                                EnemyType = EnemyType.Grenadier,
                                Count = 10,
                                Cooldown = 5f
                            },
                        }
                    }
                }
            };
        }

        public sealed class Wave : ISerializable
        {
            public short Version;
            public List<SpawnSequence> Sequences;

            public short GetLenght()
            {
                var lenght = sizeof(short) + SerializationUtils.GetSizeOfList(Sequences);
                return (short)lenght;
            }

            public void Serialize(byte[] data, ref int offset)
            {
                offset += ByteConverter.AddToStream(Version, data, offset);
                SerializationUtils.SerializeList(Sequences, data, ref offset);
            }

            public void Deserialize(byte[] data, ref int offset)
            {
                offset += ByteConverter.ReturnFromStream(data, offset, out Version);
                Sequences = SerializationUtils.DeserializeList<SpawnSequence>(data, ref offset);
            }
        }

        public sealed class SpawnSequence : ISerializable
        {
            public EnemyType EnemyType;
            public short Count;
            public float Cooldown;

            public short GetLenght()
            {
                var lenght = sizeof(byte) + sizeof(short) + sizeof(float);
                return (short)lenght;
            }
            
            public void Serialize(byte[] data, ref int offset)
            {
                offset += ByteConverter.AddToStream((byte)EnemyType, data, offset);
                offset += ByteConverter.AddToStream(Count, data, offset);
                offset += ByteConverter.AddToStream(Cooldown, data, offset);
            }

            public void Deserialize(byte[] data, ref int offset)
            {
                offset += ByteConverter.ReturnFromStream(data, offset, out byte enemyType);
                EnemyType = (EnemyType)enemyType;
                offset += ByteConverter.ReturnFromStream(data, offset, out Count);
                offset += ByteConverter.ReturnFromStream(data, offset, out Cooldown);
            }
        }
    }
}
