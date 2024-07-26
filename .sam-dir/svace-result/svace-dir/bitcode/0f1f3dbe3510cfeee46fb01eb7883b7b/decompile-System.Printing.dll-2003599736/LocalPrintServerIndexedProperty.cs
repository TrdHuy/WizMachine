namespace System.Printing;

/// <summary>Specifies the properties of a <see cref="T:System.Printing.LocalPrintServer" /> object that are initialized when it is constructed.</summary>
public enum LocalPrintServerIndexedProperty
{
	/// <summary>The property that specifies whether a printer error causes the local print server to beep.</summary>
	BeepEnabled = 5,
	/// <summary>The property that specifies the default priority for the thread that manages port I/0.</summary>
	DefaultPortThreadPriority = 2,
	/// <summary>The property that specifies the default print queue for the local print server.</summary>
	DefaultPrintQueue = 12,
	/// <summary>The property that specifies the default priority for the scheduler.</summary>
	DefaultSchedulerPriority = 4,
	/// <summary>The property that specifies the path to the folder where spool jobs are located as temporary files.</summary>
	DefaultSpoolDirectory = 0,
	/// <summary>The property that specifies the kind of event logging that is provided by the local print server.</summary>
	EventLog = 7,
	/// <summary>The property that specifies the major version of the operating system.</summary>
	MajorVersion = 8,
	/// <summary>The property that specifies the minor version of the operating system.</summary>
	MinorVersion = 9,
	/// <summary>The property that specifies whether the client computer or the print server receives notifications that a job is finished.</summary>
	NetPopup = 6,
	/// <summary>The property that specifies the priority of the thread that manages port I/O.</summary>
	PortThreadPriority = 1,
	/// <summary>The property that specifies whether users can restart jobs when printer pooling is being used.</summary>
	RestartJobOnPoolEnabled = 11,
	/// <summary>The property that specifies the wait time before jobs can be restarted when printer pooling is being used.</summary>
	RestartJobOnPoolTimeout = 10,
	/// <summary>The property that specifies the priority of the scheduler.</summary>
	SchedulerPriority = 3
}
