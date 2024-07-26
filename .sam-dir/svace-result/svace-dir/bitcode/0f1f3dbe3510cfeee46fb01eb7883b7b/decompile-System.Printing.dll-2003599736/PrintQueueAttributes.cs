namespace System.Printing;

/// <summary>Specifies the attributes of a print queue and its printer.</summary>
[Flags]
public enum PrintQueueAttributes
{
	/// <summary>The print queue sends print jobs immediately to the printer instead of spooling jobs first.</summary>
	Direct = 2,
	/// <summary>The printer's bidirectional communication is enabled.</summary>
	EnableBidi = 0x800,
	/// <summary>The queue holds its jobs when the document and printer configurations do not match.</summary>
	EnableDevQuery = 0x80,
	/// <summary>The print queue is not visible in the application UI.</summary>
	Hidden = 0x20,
	/// <summary>The printer language file is not deleted after the file prints.</summary>
	KeepPrintedJobs = 0x100,
	/// <summary>No print queue attribute is specified.</summary>
	None = 0,
	/// <summary>The print queue is visible to other network users.</summary>
	Published = 0x2000,
	/// <summary>The print queue can hold more than one print job at a time.</summary>
	Queued = 1,
	/// <summary>The print queue cannot use enhanced metafile (EMF) printing.</summary>
	RawOnly = 0x1000,
	/// <summary>The queue prints a fully spooled job before it prints higher priority jobs that are still spooling.</summary>
	ScheduleCompletedJobsFirst = 0x200,
	/// <summary>The print queue is shared.</summary>
	Shared = 8
}
