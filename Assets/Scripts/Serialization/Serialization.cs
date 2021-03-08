public class Serialization
{
    public const int VERSION = 1;
    public const string DEFEND_PATH = "DefenseFiles";
    public const string DEFEND_EXTENSION = ".def";
        
    public enum ErrorCode
    {
        FileNotFound,
        VersionInvalid,
        Unknown
    }
}