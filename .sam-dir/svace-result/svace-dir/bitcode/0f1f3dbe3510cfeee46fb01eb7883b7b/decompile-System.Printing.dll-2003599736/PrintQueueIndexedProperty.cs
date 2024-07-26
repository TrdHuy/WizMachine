namespace System.Printing;

/// <summary>Specifies the properties that are initialized when a <see cref="T:System.Printing.PrintQueue" /> object is constructed.</summary>
public enum PrintQueueIndexedProperty
{
	/// <summary>The speed of the print queue.</summary>
	AveragePagesPerMinute = 9,
	/// <summary>A comment specific to the print queue.</summary>
	Comment = 2,
	/// <summary>The default print ticket object.</summary>
	DefaultPrintTicket = 19,
	/// <summary>The default priority.</summary>
	DefaultPriority = 6,
	/// <summary>The description of the queue.</summary>
	Description = 4,
	/// <summary>The host print server.</summary>
	HostingPrintServer = 15,
	/// <summary>The location of the physical printer.</summary>
	Location = 3,
	/// <summary>The name of the print queue.</summary>
	Name = 0,
	/// <summary>The number of jobs in the print queue.</summary>
	NumberOfJobs = 10,
	/// <summary>The priority of the print queue relative to other print queues serving the same printer.</summary>
	Priority = 5,
	/// <summary>The attributes of the print queue.</summary>
	QueueAttributes = 11,
	/// <summary>The printer driver for the queue.</summary>
	QueueDriver = 12,
	/// <summary>The printer port used by the print queue.</summary>
	QueuePort = 13,
	/// <summary>The print processor for the print queue.</summary>
	QueuePrintProcessor = 14,
	/// <summary>The current status of the queue.</summary>
	QueueStatus = 16,
	/// <summary>The path to the separator file.</summary>
	SeparatorFile = 17,
	/// <summary>The share name of the queue.</summary>
	ShareName = 1,
	/// <summary>The time of day that the queue begins printing its jobs.</summary>
	StartTimeOfDay = 7,
	/// <summary>The time of day that the queue stops printing jobs.</summary>
	UntilTimeOfDay = 8,
	/// <summary>The print ticket for the user.</summary>
	UserPrintTicket = 18
}
