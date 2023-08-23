

namespace Core
{
    public sealed class UserContainer
    {
        public UserAccountState State { get; set; }
        public AppConfiguration Configuration { get; set; }

        public bool IsBuildAllowed(GameTileContentType tileContentType)
        {
            return State.Currencies.Crystals >= Configuration.GetBuildCost(tileContentType);
        }

        public bool IsUpgradeAllowed(GameTileContentType tileContentType, int level)
        {
            return State.Currencies.Gas >= Configuration.GetUpgradeCost(tileContentType, level);
        }

        public void SpendAfterBuild(GameTileContentType tileContentType)
        {
            var buildCost = Configuration.GetBuildCost(tileContentType);
            State.Currencies.ChangeCrystals(-buildCost);
        }

        public void SpendAfterUpgrade(GameTileContentType tileContentType, int level)
        {
            var upgradeCost = Configuration.GetUpgradeCost(tileContentType, level);
            State.Currencies.ChangeGas(-upgradeCost);
        }
    }
}
