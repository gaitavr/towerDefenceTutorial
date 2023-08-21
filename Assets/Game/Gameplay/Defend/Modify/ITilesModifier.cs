namespace Game.Defend.Tiles
{
    public interface ITilesModifier
    {
        void DoWithTile(TileModifyActions actionType);
    }
}