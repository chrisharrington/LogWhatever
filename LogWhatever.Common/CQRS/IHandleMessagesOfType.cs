namespace LogWhatever.Common.CQRS
{
    public interface IHandleMessagesOfType<T> : IHandleMessages
    {
        void Handle(T message);
    }
}
