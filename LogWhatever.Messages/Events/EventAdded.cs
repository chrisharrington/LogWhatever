


using System;

namespace LogWhatever.Messages.Events 
{
	public class  EventAdded : BaseEvent {
			 	
			public System.Guid UserId {get;set;}
		   
			 	
			public System.Guid LogId {get;set;}
		   
			 	
			public System.String LogName {get;set;}
		   
			 	
			public System.DateTime Date {get;set;}
		   
			  
	 
	}
}
