namespace Utils.Serialization
{
    public class Constants
    {
        public const string DEFEND_PATH = "DefenseFiles";
        public const string DEFEND_EXTENSION = ".def";

        public enum ErrorCode
        {
            FileNotFound,
            VersionInvalid,
            Unknown
        }
    }
}