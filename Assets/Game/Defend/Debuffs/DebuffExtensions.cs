namespace Defend.Debuffs
{
    public static class DebuffExtensions
    {
        public static IDebuff GetDebuff(this GameTileContentType contentType, GameTileContentFactory factory, int level)
        {
            switch (contentType)
            {
                case GameTileContentType.Ice: return new IceSlower(factory.IceConfig, level);
                case GameTileContentType.Lava: return new LavaObstacle();
                default: return new EmptyDebuff();
            }
        }
    }
}