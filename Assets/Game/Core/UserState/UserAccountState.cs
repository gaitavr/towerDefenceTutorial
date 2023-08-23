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
        public List<BoardData> Boards;

        public byte[] Serialize()
        {
            var boards = new List<byte>(100);

            if (Boards != null)
            {
                boards.Add((byte)Boards.Count);
                foreach (var board in Boards)
                {
                    var serializedBoard = board.Serialize();
                    boards.AddRange(ByteConverter.Serialize(serializedBoard.Length));
                    boards.AddRange(serializedBoard);
                }
            }

            var socialBytes = Social.Serialize();
            var currenciesBytes = Currencies.Serialize();
            var result = new byte[sizeof(int) * 2 + sizeof(int) + socialBytes.Length + sizeof(int) + currenciesBytes.Length
                + sizeof(byte) + boards.Count];

            var offset = 0;
            offset += ByteConverter.AddToStream(Version, result, offset);
            offset += ByteConverter.AddToStream(Id, result, offset);

            offset += ByteConverter.AddToStream(socialBytes.Length, result, offset);
            offset += ByteConverter.AddToStream(socialBytes, result, offset);

            offset += ByteConverter.AddToStream(currenciesBytes.Length, result, offset);
            offset += ByteConverter.AddToStream(currenciesBytes, result, offset);

            offset += ByteConverter.AddToStream(boards.ToArray(), result, offset);

            return result;
        }

        public void Deserialize(byte[] data)
        {
            var offset = 0;

            offset += ByteConverter.ReturnFromStream(data, offset, out Version);
            offset += ByteConverter.ReturnFromStream(data, offset, out Id);

            Social = new UserSocialState();
            offset += ByteConverter.ReturnFromStream(data, offset, out int socialSize);
            offset += ByteConverter.ReturnFromStream(data, offset, socialSize, out var socialBytes);
            Social.Deserialize(socialBytes);

            Currencies = new UserCurrenciesState();
            offset += ByteConverter.ReturnFromStream(data, offset, out int currenciesSize);
            offset += ByteConverter.ReturnFromStream(data, offset, currenciesSize, out var currenciesBytes);
            Currencies.Deserialize(currenciesBytes);
            
            offset += ByteConverter.ReturnFromStream(data, offset, out byte boardsCount);
            Boards = new List<BoardData>(boardsCount);
            for (int i = 0; i < boardsCount; i++)
            {
                offset += ByteConverter.ReturnFromStream(data, offset, out int boardSize);
                offset += ByteConverter.ReturnFromStream(data, offset, boardSize, out var boardBytes);
                var board = new BoardData();
                board.Deserialize(boardBytes);
                Boards.Add(board);
            }
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
                    Crystals = 10000,
                    Gas = 125
                },
                Boards = new List<BoardData>()
            };
        }

        public BoardData TryGetBoard(string boardName)
        {
            return Boards.FirstOrDefault(b => b.Name == boardName);
        }

        public void AddOrReplaceBoard(BoardData board)
        {
            var index = Boards.IndexOf(board);
            if (index == -1)
                Boards.Add(board);
            else
                Boards[index] = board;
        }

        public bool TryDeleteBoard(string boardName)
        {
            var boardToDelete = TryGetBoard(boardName);
            if (boardToDelete == null)
                return false;

            Boards.Remove(boardToDelete);
            return true;
        }

        public bool IsValid() => Id > 0;
    }
}