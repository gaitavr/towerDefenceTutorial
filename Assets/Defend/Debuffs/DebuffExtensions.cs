namespace Defend.Debuffs
{
    public static class DebuffExtensions
    {
        public static IDebuff GetDebuff(this GameTileContentType contentType)
        {
            switch (contentType)
            {
                case GameTileContentType.Ice: return new IceSlower();
                case GameTileContentType.Fire: return new FireDamager();
                default: return new EmptyDebuff();
            }
        }
    }
}