namespace System.Printing;

/// <summary>Specifies the status of a print queue or its printer.</summary>
[Flags]
public enum PrintQueueStatus
{
	/// <summary>The printer is busy.</summary>
	Busy = 0x200,
	/// <summary>A door on the printer is open.</summary>
	DoorOpen = 0x400000,
	/// <summary>The printer cannot print due to an error condition.</summary>
	Error = 2,
	/// <summary>The printer is initializing.</summary>
	Initializing = 0x8000,
	/// <summary>The printer is exchanging data with the print server.</summary>
	IOActive = 0x100,
	/// <summary>The printer is waiting for a user to place print media in the manual feed bin.</summary>
	ManualFeed = 0x20,
	/// <summary>Status is not specified.</summary>
	None = 0,
	/// <summary>Status information is unavailable.</summary>
	NotAvailable = 0x1000,
	/// <summary>The printer is out of toner.</summary>
	NoToner = 0x40000,
	/// <summary>The printer is offline.</summary>
	Offline = 0x80,
	/// <summary>The printer has no available memory.</summary>
	OutOfMemory = 0x200000,
	/// <summary>The printer's output bin is full.</summary>
	OutputBinFull = 0x800,
	/// <summary>The printer is unable to print the current page.</summary>
	PagePunt = 0x80000,
	/// <summary>The paper in the printer is jammed.</summary>
	PaperJam = 8,
	/// <summary>The printer does not have, or is out of, the type of paper needed for the current print job.</summary>
	PaperOut = 0x10,
	/// <summary>The paper in the printer is causing an unspecified error condition.</summary>
	PaperProblem = 0x40,
	/// <summary>The print queue is paused.</summary>
	Paused = 1,
	/// <summary>The print queue is deleting a print job.</summary>
	PendingDeletion = 4,
	/// <summary>The printer is in power save mode.</summary>
	PowerSave = 0x1000000,
	/// <summary>The device is printing.</summary>
	Printing = 0x400,
	/// <summary>The device is doing some kind of work, which need not be printing if the device is a combination printer, fax machine, and scanner.</summary>
	Processing = 0x4000,
	/// <summary>The printer is in an error state.</summary>
	ServerUnknown = 0x800000,
	/// <summary>Only a small amount of toner remains in the printer.</summary>
	TonerLow = 0x20000,
	/// <summary>The printer requires user action to correct an error condition.</summary>
	UserIntervention = 0x100000,
	/// <summary>The printer is waiting for a print job.</summary>
	Waiting = 0x2000,
	/// <summary>The printer is warming up.</summary>
	WarmingUp = 0x10000
}
