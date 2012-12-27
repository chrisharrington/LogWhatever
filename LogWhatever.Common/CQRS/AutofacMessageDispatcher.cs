using System;
using Autofac;
using System.Linq;
using System.Collections.Generic;

namespace LogWhatever.Common.CQRS
{
	public class AutofacMessageDispatcher : IMessageDispatcher
	{
		#region Properties
		public IComponentContext ComponentContext { get; set; }
		#endregion

		#region Public Methods
		public void Dispatch(object message)
		{
			if (message == null)
				throw new ArgumentNullException("message");

			var type = message.GetType();
			while (type != null)
			{
				ExecuteHandlers(message, Resolve(ConstructListType(type)));
				type = type.BaseType;
			}
		}
		#endregion

		#region Private Methods
		internal virtual IEnumerable<object> Resolve(Type type)
		{
			return ComponentContext.Resolve(type) as IEnumerable<object>;
		}

		internal virtual Type ConstructListType(Type type)
		{
			return typeof(IEnumerable<>).MakeGenericType(typeof(IHandleMessagesOfType<>).MakeGenericType(type));
		}

		internal virtual void ExecuteHandlers(object message, IEnumerable<object> handlers)
		{
			foreach (var handler in handlers)
			{
				var method = handler.GetType().GetMethods().FirstOrDefault(x => x.Name == "Handle" && x.GetParameters().First().ParameterType == message.GetType());
				if (method == null)
					return;

				method.Invoke(handler, new[] {message});
			}
		}
		#endregion
	}
}