namespace System.Printing;

/// <summary>Specifies attributes of print queues.</summary>
[Flags]
public enum EnumeratedPrintQueueTypes
{
	/// <summary>A print queue that is connected to the specified print server.</summary>
	Connections = 0x10,
	/// <summary>A print queue that sends a print job directly to printing instead of spooling the job first.</summary>
	DirectPrinting = 2,
	/// <summary>A print queue for a printer that has bidirectional communication enabled.</summary>
	EnableBidi = 0x800,
	/// <summary>A print queue that holds its print jobs when the document and printer configurations do not match.</summary>
	EnableDevQuery = 0x80,
	/// <summary>A print queue that services a fax machine.</summary>
	Fax = 0x4000,
	/// <summary>A print queue that keeps jobs in the queue after printing them.</summary>
	KeepPrintedJobs = 0x100,
	/// <summary>A print queue that is installed as a local print queue on the specified print server.</summary>
	Local = 0x40,
	/// <summary>A print queue that is visible in the directory of printers.</summary>
	PublishedInDirectoryServices = 0x2000,
	/// <summary>A print queue that was installed by using the Push Printer Connections computer policy.</summary>
	PushedMachineConnection = 0x40000,
	/// <summary>A print queue that was installed by using the Push Printer Connections user policy.</summary>
	PushedUserConnection = 0x20000,
	/// <summary>A print queue that allows multiple print jobs in the queue.</summary>
	Queued = 1,
	/// <summary>A print queue that spools only raw data.</summary>
	RawOnly = 0x1000,
	/// <summary>A print queue that is shared.</summary>
	Shared = 8,
	/// <summary>A print queue that is installed by the redirection feature in Terminal Services.</summary>
	TerminalServer = 0x8000,
	/// <summary>A print queue that can work offline.</summary>
	WorkOffline = 0x400
}
