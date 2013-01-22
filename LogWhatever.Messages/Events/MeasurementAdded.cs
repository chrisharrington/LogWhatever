using System;

namespace LogWhatever.Messages.Events 
{
	public class  MeasurementAdded : BaseEvent {
			 	
			public System.String Name {get;set;}
		   
			 	
			public System.Guid UserId {get;set;}
		   
			 	
			public System.Guid LogId {get;set;}
		   
			 	
			public System.String LogName {get;set;}
		   
			 	
			public System.Guid EventId {get;set;}
		   
			 	
			public System.Decimal Quantity {get;set;}
		   
			 	
			public System.String Unit {get;set;}
		   
			 	
			public System.DateTime Date {get;set;}
		   
			  
	 
	}
}
