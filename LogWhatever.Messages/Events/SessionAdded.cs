using System;

namespace LogWhatever.Messages.Events 
{
	public class  SessionAdded : BaseEvent {
			 	
			public System.Guid UserId {get;set;}
		   
			 	
			public System.String Name {get;set;}
		   
			 	
			public System.String EmailAddress {get;set;}
		   
			  
	 
	}
}
