using System.Text;
using Utils.Serialization;

namespace Core
{
    public sealed class UserAttackScenarioState : ISerializable
    {
        public int Version;
        public string Name;

        public byte[] Serialize()
        {
            var nameBytes = Encoding.UTF8.GetBytes(Name);
            var result = new byte[sizeof(int) + sizeof(byte) + nameBytes.Length];

            return result;
        }

        public void Deserialize(byte[] data)
        {
            
        }
    }
}
