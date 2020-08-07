namespace TheFlyingDragons
{
    public interface IPlayerInventoryItem : IPlayerOwned
    {
        bool IsPassive { get; }
    }
}