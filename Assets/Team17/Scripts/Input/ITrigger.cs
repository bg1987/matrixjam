namespace TheFlyingDragons
{
    public interface ITrigger : IInputHandler
    {
        void TriggerDown();
        void TriggerUp();
    }
}