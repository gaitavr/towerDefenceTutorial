namespace Defend.Debuffs
{
    public static class DebuffExtensions
    {
        public static IDebuff GetDebuff(this GameTileContentType contentType, GameTileContentFactory factory, int level)
        {
            switch (contentType)
            {
                case GameTileContentType.Ice: return new IceDebuff(factory.IceConfig, level);
                case GameTileContentType.Lava: return new LavaDebuff(factory.LavaConfig, level);
                default: return new EmptyDebuff();
            }
        }
    }
}