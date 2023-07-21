using System.Collections.Generic;

namespace Defend.Debuffs
{
    public static class DebuffExtensions
    {
        private static Dictionary<GameTileContentType, IDebuff> _debuffs
         = new Dictionary<GameTileContentType, IDebuff>()
         {
             [GameTileContentType.Ice] = new IceSlower()
         }
        
        public static IDebuff GetDebuff(this GameTileContentType contentType, GameTileContentFactory factory, int level)
        {
            switch (contentType)
            {
                case GameTileContentType.Ice: return new IceSlower(factory.IceConfig, level);
                case GameTileContentType.Lava: return new LavaObstacle(factory.LavaConfig, level);
                default: return new EmptyDebuff();
            }
        }
    }
}