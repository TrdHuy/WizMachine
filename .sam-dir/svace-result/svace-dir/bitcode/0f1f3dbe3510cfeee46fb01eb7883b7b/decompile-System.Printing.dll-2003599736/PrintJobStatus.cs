namespace System.Printing;

/// <summary>Specifies the current status of a print job in a print queue.</summary>
[Flags]
public enum PrintJobStatus
{
	/// <summary>An error condition, possibly on a print job that precedes this one in the queue, blocked the print job.</summary>
	Blocked = 0x200,
	/// <summary>The print job is complete, including any post-printing processing.</summary>
	Completed = 0x1000,
	/// <summary>The print job was deleted from the queue, typically after printing.</summary>
	Deleted = 0x100,
	/// <summary>The print job is in the process of being deleted.</summary>
	Deleting = 4,
	/// <summary>The print job is in an error state.</summary>
	Error = 2,
	/// <summary>The print job has no specified state.</summary>
	None = 0,
	/// <summary>The printer is offline.</summary>
	Offline = 0x20,
	/// <summary>The printer is out of the required paper size.</summary>
	PaperOut = 0x40,
	/// <summary>The print job is paused.</summary>
	Paused = 1,
	/// <summary>The print job printed.</summary>
	Printed = 0x80,
	/// <summary>The print job is now printing.</summary>
	Printing = 0x10,
	/// <summary>The print job was blocked but has restarted.</summary>
	Restarted = 0x800,
	/// <summary>The print job is retained in the print queue after printing.</summary>
	Retained = 0x2000,
	/// <summary>The print job is spooling.</summary>
	Spooling = 8,
	/// <summary>The printer requires user action to fix an error condition.</summary>
	UserIntervention = 0x400
}
