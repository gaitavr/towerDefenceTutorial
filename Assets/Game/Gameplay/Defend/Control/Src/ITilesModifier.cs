namespace GamePlay.Defend
{
    public interface ITilesModifier
    {
        void DoWithTile(TileModifyActions actionType);
    }
}