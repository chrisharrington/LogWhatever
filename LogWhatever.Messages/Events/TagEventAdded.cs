using System;

namespace LogWhatever.Messages.Events 
{
	public class  TagEventAdded : BaseEvent {
			 	
			public System.Guid UserId {get;set;}
		   
			 	
			public System.Guid TagId {get;set;}
		   
			 	
			public System.Guid EventId {get;set;}
		   
			 	
			public System.String Name {get;set;}
		   
			 	
			public System.Guid LogId {get;set;}
		   
			 	
			public System.String LogName {get;set;}
		   
			  
	 
	}
}
