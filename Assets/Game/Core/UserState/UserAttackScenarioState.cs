using GamePlay.Attack;
using System;
using System.Collections.Generic;
using Utils.Serialization;

namespace Core
{
    public sealed class UserAttackScenarioState : ISerializable
    {
        public int Version;
        public DateTime CreationDate;
        public List<Wave> Waves;

        public byte[] Serialize()
        {
            var wavesBytes = SerializationUtils.SerializeList(Waves);
            var result = new byte[
                sizeof(int) 
                + sizeof(long)
                + wavesBytes.Count];

            var offset = 0;
            offset += ByteConverter.AddToStream(Version, result, offset);
            offset += ByteConverter.AddToStream(CreationDate.Ticks, result, offset);
            offset += ByteConverter.AddToStream(wavesBytes, result, offset);

            return result;
        }

        public void Deserialize(byte[] data)
        {
            var offset = 0;

            offset += ByteConverter.ReturnFromStream(data, offset, out Version);
            offset += ByteConverter.ReturnFromStream(data, offset, out ulong ticks);
            CreationDate = new DateTime((long)ticks);
            Waves = SerializationUtils.DeserializeList<Wave>(data, ref offset);
        }

        public static UserAttackScenarioState GetInitial()
        {
            return new UserAttackScenarioState()
            {
                Version = 1,
                CreationDate = DateTime.Now,
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
            public int Version;
            public List<SpawnSequence> Sequences;

            public byte[] Serialize()
            {
                var sequencesBytes = SerializationUtils.SerializeList(Sequences);

                var result = new byte[
                    sizeof(int) + sequencesBytes.Count];

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
            public EnemyType EnemyType;
            public int Count;
            public float Cooldown;

            public byte[] Serialize()
            {
                var result = new byte[
                    + sizeof(byte)
                    + sizeof(int)
                    + sizeof(float)];

                var offset = 0;
                offset += ByteConverter.AddToStream((byte)EnemyType, result, offset);
                offset += ByteConverter.AddToStream(Count, result, offset);
                offset += ByteConverter.AddToStream(Cooldown, result, offset);

                return result;
            }

            public void Deserialize(byte[] data)
            {
                var offset = 0;

                offset += ByteConverter.ReturnFromStream(data, offset, out byte enemyType);
                EnemyType = (EnemyType)enemyType;
                offset += ByteConverter.ReturnFromStream(data, offset, out Count);
                offset += ByteConverter.ReturnFromStream(data, offset, out Cooldown);
            }
        }
    }
}
