using System;

namespace LogWhatever.Messages.Events 
{
	public class  MeasurementValueAdded : BaseEvent {
			 	
			public System.Guid LogId {get;set;}
		   
			 	
			public System.Guid UserId {get;set;}
		   
			 	
			public System.Guid MeasurementId {get;set;}
		   
			 	
			public System.Decimal Quantity {get;set;}
		   
			  
	 
	}
}
