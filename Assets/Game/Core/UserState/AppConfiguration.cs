namespace Core
{
    public sealed class AppConfiguration
    {
        public int GetBuildCost(GameTileContentType tileContentType)
        {
            switch (tileContentType)
            {
                case GameTileContentType.Ice:
                    return 350;
                case GameTileContentType.Lava:
                    return 200;
                case GameTileContentType.LaserTower:
                    return 400;
                case GameTileContentType.MortarTower:
                    return 500;
                case GameTileContentType.Wall:
                    return 25;
            }
            return 0;
        }

        public int GetUpgradeCost(GameTileContentType tileContentType, int level)
        {
            switch (tileContentType)
            {
                case GameTileContentType.Ice:
                    return 50 * level;
                case GameTileContentType.Lava:
                    return 35 * level;
                case GameTileContentType.LaserTower:
                    return 75 * level;
                case GameTileContentType.MortarTower:
                    return 100 * level;
            }
            return 0;
        }
    }
}