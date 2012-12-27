using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using LogWhatever.Common.CQRS;
using LogWhatever.Common.Extensions;
using LogWhatever.Messages.Events;

namespace LogWhatever.Handlers.Commands
{
    public class BaseCommandHandler
	{
		#region Properties
		public IMessageDispatcher MessageDispatcher { get; set; }
		#endregion

		#region Protected Methods
		protected internal void CreateAndDispatchEvent(object command)
        {
			if (command == null)
				throw new ArgumentNullException("command");

            MessageDispatcher.Dispatch(CreateMessage(command));
        }
		#endregion

		#region Private Methods
		protected internal virtual object CreateMessage(object command)
        {
            var eventName = RenameCommandToEvent(command.GetType().Name);
            var eventToCreate = typeof(BaseEvent).Assembly.GetTypes().FirstOrDefault(x => x.Name == eventName);
            return eventToCreate != null ? command.Map(eventToCreate) : null;
        }

		protected internal virtual string RenameCommandToEvent(string commandName)
        {
            var action = GetPastTense(GetWordsFromCamelCase(commandName).First());
            var item = string.Join("", GetWordsFromCamelCase(commandName).Skip(1));
            var itemObject = "";
            if (item.Contains("To"))
            {
                itemObject = "To" + item.Substring(0, item.IndexOf("To", StringComparison.Ordinal));
                item = item.Substring(item.IndexOf("To", StringComparison.Ordinal) + 2);
            }
            if (item.Contains("From"))
            {
                itemObject = "From" + item.Substring(item.IndexOf("From", StringComparison.Ordinal) + 4);
                item = item.Substring(0, item.IndexOf("From", StringComparison.Ordinal)); 
            }
            return item + action + itemObject;
        }

        //this method is, incidentally, bullshit
		protected internal virtual string GetPastTense(string toPasttensify)
        {
            if (toPasttensify == "Set")
                return "Set";
            if (toPasttensify.EndsWith("e"))
                return toPasttensify + "d";
            return toPasttensify + "ed";
        }

		protected internal virtual IEnumerable<string> GetWordsFromCamelCase(string camelCaseString)
        {
            var sb = new StringBuilder();
            foreach (var c in camelCaseString)
            {
                if (Char.IsUpper(c))
                    sb.Append(" ");
                sb.Append(c);
            }
            var workingString = sb.ToString().Trim();
            return workingString.Split(' ');
        }
		#endregion
    }
}
