namespace ConnectionActions
{
    public interface IComponentAction
    {
        void OnValidConnection();
        void OnInvalidConnection();
    }
}