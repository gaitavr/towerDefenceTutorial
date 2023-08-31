

namespace Core
{
    public sealed class UserContainer
    {
        public UserAccountState State { get; set; }
        public AppConfiguration Configuration { get; set; }

        public bool IsFreeTiles { get; set; }

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
            ChangeAfterBuild(tileContentType, false);
        }

        public void RefundAfterBuild(GameTileContentType tileContentType)
        {
            ChangeAfterBuild(tileContentType, true);
        }

        private void ChangeAfterBuild(GameTileContentType tileContentType, bool isPositive)
        {
            if (IsFreeTiles)
                return;
            var buildCost = Configuration.GetBuildCost(tileContentType);
            if (isPositive == false)
                buildCost *= -1;
            State.Currencies.ChangeCrystals(buildCost);
        }

        public void SpendAfterUpgrade(GameTileContentType tileContentType, int level)
        {
            ChangeAfterUpgrade(tileContentType, level, false);
        }

        public void RefundAfterUpgrade(GameTileContentType tileContentType, int level)
        {
            ChangeAfterUpgrade(tileContentType, level, true);
        }

        private void ChangeAfterUpgrade(GameTileContentType tileContentType, int level, bool isPositive)
        {
            if (IsFreeTiles)
                return;
            var upgradeCost = Configuration.GetUpgradeCost(tileContentType, level);
            if (isPositive == false)
                upgradeCost *= -1;
            State.Currencies.ChangeGas(upgradeCost);
        }
    }
}
