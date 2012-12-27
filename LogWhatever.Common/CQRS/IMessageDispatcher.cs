namespace LogWhatever.Common.CQRS
{
    public interface IMessageDispatcher
	{
		#region Public Methods
		void Dispatch(object message);
		#endregion
	}
}
