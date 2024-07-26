using System.IO;
using System.Windows.Controls;
using System.Windows.Xps;
using System.Windows.Xps.Serialization.RCW;

namespace System.Printing;

/// <summary>Manages printers and print jobs.</summary>
public class PrintQueue : PrintSystemObject
{
	/// <summary>Gets the speed of the printer measured in pages per minute.</summary>
	/// <returns>The average pages printed per minute of the printer.</returns>
	public virtual int AveragePagesPerMinute
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets the version of the Print Schema.</summary>
	/// <returns>The version of the Print Schema in use.</returns>
	public int ClientPrintSchemaVersion
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets or sets a comment about the printer.</summary>
	/// <returns>A comment about the printer.</returns>
	public virtual string Comment
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets an object that contains the configuration settings for the current print job.</summary>
	/// <returns>A <see cref="T:System.Printing.PrintJobSettings" /> value that holds the settings of the currently printing job. These settings include a description of the job and a reference to the job's <see cref="T:System.Printing.PrintTicket" />.</returns>
	public PrintJobSettings CurrentJobSettings
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets or sets the default printer options associated with this <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <returns>The default <see cref="T:System.Printing.PrintTicket" /> for the print queue; or <see langword="null" /> if an error has occurred in the print queue.</returns>
	public virtual PrintTicket DefaultPrintTicket
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets or sets the default priority that is given to each new print job added to the queue.</summary>
	/// <returns>The default priority for print jobs added to the queue. Possible values range from 1 through 99. The default is 1.</returns>
	public virtual int DefaultPriority
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets a description of the print queue.</summary>
	/// <returns>A description of the print queue.</returns>
	public virtual string Description
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets the complete name of the queue.</summary>
	/// <returns>The complete name of the print queue.</returns>
	public string FullName
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates if the printer is having an unspecified paper problem.</summary>
	/// <returns>
	///   <see langword="true" /> if there is an unspecified paper problem; otherwise, <see langword="false" />.</returns>
	public bool HasPaperProblem
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates if the printer has toner.</summary>
	/// <returns>
	///   <see langword="true" /> if the current printer has toner; otherwise, <see langword="false" />.</returns>
	public bool HasToner
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets or sets (protected) the print server that controls the print queue.</summary>
	/// <returns>The name and other properties of the <see cref="T:System.Printing.PrintServer" /> that is hosting the print queue.</returns>
	public virtual PrintServer HostingPrintServer
	{
		get
		{
			throw null;
		}
		protected set
		{
		}
	}

	/// <summary>Gets or sets a value that indicates whether the queue is operating in a partially trusted mode, a higher level of trust.</summary>
	/// <returns>
	///   <see langword="true" /> if the queue is operating in a partially trusted mode; otherwise, <see langword="false" />.</returns>
	public bool InPartialTrust
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets a value that indicates whether bidirectional communication with the printer is enabled.</summary>
	/// <returns>
	///   <see langword="true" /> if bidirectional communication with the printer is enabled; otherwise, <see langword="false" />.</returns>
	public bool IsBidiEnabled
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the printing device is busy.</summary>
	/// <returns>
	///   <see langword="true" /> if the device is busy; otherwise, <see langword="false" />.</returns>
	public bool IsBusy
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the queue holds documents when document and printer configurations do not match.</summary>
	/// <returns>
	///   <see langword="true" /> if the queue holds mismatched configurations; otherwise, <see langword="false" />.</returns>
	public bool IsDevQueryEnabled
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the queue prints directly to the printer or spools documents first and then prints them.</summary>
	/// <returns>
	///   <see langword="true" /> if the queue prints directly to the printer; otherwise, <see langword="false" />.</returns>
	public bool IsDirect
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether a door is open on the printer.</summary>
	/// <returns>
	///   <see langword="true" /> if a door is open; otherwise, <see langword="false" />.</returns>
	public bool IsDoorOpened
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the print queue is hidden in your application's user interface.</summary>
	/// <returns>
	///   <see langword="true" /> if the print queue is hidden in the Windows user interface; otherwise, <see langword="false" />.</returns>
	public bool IsHidden
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the printer or device is in an error condition.</summary>
	/// <returns>
	///   <see langword="true" /> if the device is in an error condition; otherwise, <see langword="false" />.</returns>
	public bool IsInError
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the printer is initializing itself.</summary>
	/// <returns>
	///   <see langword="true" /> if the printer is initializing; otherwise, <see langword="false" />.</returns>
	public bool IsInitializing
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the printer is receiving or sending data or signals.</summary>
	/// <returns>
	///   <see langword="true" /> if the printer is receiving or sending; otherwise, <see langword="false" />.</returns>
	public bool IsIOActive
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the printer needs to be manually fed paper for the current print job.</summary>
	/// <returns>
	///   <see langword="true" /> if the printer needs to be manually fed paper; otherwise, <see langword="false" />.</returns>
	public bool IsManualFeedRequired
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the printer is available.</summary>
	/// <returns>
	///   <see langword="true" /> if the printer is available; otherwise, <see langword="false" />.</returns>
	public bool IsNotAvailable
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the printer is offline.</summary>
	/// <returns>
	///   <see langword="true" /> if the printer is offline; otherwise, <see langword="false" />.</returns>
	public bool IsOffline
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the printer is out of memory.</summary>
	/// <returns>
	///   <see langword="true" /> if the printer is out of memory; otherwise, <see langword="false" />.</returns>
	public bool IsOutOfMemory
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the printer needs to be reloaded with paper of the size required for the current job.</summary>
	/// <returns>
	///   <see langword="true" /> if the printer needs to be reloaded; otherwise, <see langword="false" />.</returns>
	public bool IsOutOfPaper
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the output area of the printer is in danger of overflowing.</summary>
	/// <returns>
	///   <see langword="true" /> if the output area of the printer is full; otherwise, <see langword="false" />.</returns>
	public bool IsOutputBinFull
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the current sheet of paper is stuck in the printer.</summary>
	/// <returns>
	///   <see langword="true" /> if the paper is stuck; otherwise, <see langword="false" />.</returns>
	public bool IsPaperJammed
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the print queue has been paused.</summary>
	/// <returns>
	///   <see langword="true" /> if the print queue has been paused; otherwise, <see langword="false" />.</returns>
	public bool IsPaused
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the printer is in the process of deleting a print job.</summary>
	/// <returns>
	///   <see langword="true" /> if the printer is deleting a job; otherwise, <see langword="false" />.</returns>
	public bool IsPendingDeletion
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the printer is in power save mode.</summary>
	/// <returns>
	///   <see langword="true" /> if the printer is in power save mode; otherwise, <see langword="false" />.</returns>
	public bool IsPowerSaveOn
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether a job is printing.</summary>
	/// <returns>
	///   <see langword="true" /> if a job is printing; otherwise, <see langword="false" />.</returns>
	public bool IsPrinting
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the printer is processing a print job.</summary>
	/// <returns>
	///   <see langword="true" /> if the printer is processing a print job; otherwise, <see langword="false" />.</returns>
	public bool IsProcessing
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the printer is visible to other network users.</summary>
	/// <returns>
	///   <see langword="true" /> if the printer is visible to other network users; otherwise, <see langword="false" />.</returns>
	public bool IsPublished
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the printer can support a queue with more than one print job in it at a time.</summary>
	/// <returns>
	///   <see langword="true" /> if the printer can support the queuing of multiple print jobs; otherwise, <see langword="false" />.</returns>
	public bool IsQueued
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the print queue can use EMF (Enhanced Meta File) that enables faster data flow from a printing application to the Windows spooler.</summary>
	/// <returns>
	///   <see langword="true" /> if the printer cannot use EMF printing; otherwise, <see langword="false" />.</returns>
	public bool IsRawOnlyEnabled
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the printer is in an error state.</summary>
	/// <returns>
	///   <see langword="true" /> if in the printer is in an error state; otherwise, <see langword="false" />.</returns>
	public bool IsServerUnknown
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the printer is available for use by other computers on the network.</summary>
	/// <returns>
	///   <see langword="true" /> if the printer is shared; otherwise, <see langword="false" />.</returns>
	public bool IsShared
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the printer is running short of toner.</summary>
	/// <returns>
	///   <see langword="true" /> if the printer is running short of toner; otherwise, <see langword="false" />.</returns>
	public bool IsTonerLow
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the queue is waiting for a job to be added.</summary>
	/// <returns>
	///   <see langword="true" /> if the queue is waiting for a job; otherwise, <see langword="false" />.</returns>
	public bool IsWaiting
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the printer is warming up.</summary>
	/// <returns>
	///   <see langword="true" /> if the printer is warming up; otherwise, <see langword="false" />.</returns>
	public bool IsWarmingUp
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the printer's driver is built on the XPSDrv model so it uses XML Paper Specification (XPS) as its page description language.</summary>
	/// <returns>
	///   <see langword="true" /> if the printer uses the XPS print path; otherwise, <see langword="false" />.</returns>
	public bool IsXpsDevice
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the queue is saving the printer language file instead of deleting it following printing.</summary>
	/// <returns>
	///   <see langword="true" /> if the queue is saving the printer language file; otherwise, <see langword="false" />.</returns>
	public bool KeepPrintedJobs
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets or sets the printer's physical location.</summary>
	/// <returns>The printer's physical location.</returns>
	public virtual string Location
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets the most recent possible version number of the Print Schema that the queue can use.</summary>
	/// <returns>The most recent version number of the Print Schema that the queue can use.</returns>
	public static int MaxPrintSchemaVersion
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets or sets the print queue's name.</summary>
	/// <returns>The name of the print queue.</returns>
	public sealed override string Name
	{
		get
		{
			throw null;
		}
		internal set
		{
		}
	}

	/// <summary>Gets a value that indicates whether the printer needs the attention of a human being.</summary>
	/// <returns>
	///   <see langword="true" /> if the printer needs human attention; otherwise, <see langword="false" />.</returns>
	public bool NeedUserIntervention
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets the total number of jobs lined up in the print queue.</summary>
	/// <returns>The number of jobs in the queue.</returns>
	public virtual int NumberOfJobs
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the printer is unable to print the current page.</summary>
	/// <returns>
	///   <see langword="true" /> if the printer is unable to print the current page; otherwise, <see langword="false" />.</returns>
	public bool PagePunt
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets or sets a value that indicates whether the current print job is being cancelled.</summary>
	/// <returns>
	///   <see langword="true" /> if the print job is being cancelled; otherwise, <see langword="false" />.</returns>
	public bool PrintingIsCancelled
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets or sets the priority of the print queue relative to other print queues that are hosted by the same print server and that use the same physical printer.</summary>
	/// <returns>The priority for the print queue. Possible values are from 1 through 99. The default is 1.</returns>
	public virtual int Priority
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets the properties of the print queue.</summary>
	/// <returns>A bitwise combination of the <see cref="T:System.Printing.PrintQueueAttributes" /> enumeration values.</returns>
	public PrintQueueAttributes QueueAttributes
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets or sets the printer driver for the queue.</summary>
	/// <returns>The <see cref="T:System.Printing.PrintDriver" /> that the queue uses.</returns>
	public virtual PrintDriver QueueDriver
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets or sets the port that the queue uses.</summary>
	/// <returns>The <see cref="T:System.Printing.PrintPort" /> that is assigned to the print queue.</returns>
	public virtual PrintPort QueuePort
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets or sets the print processor that the queue uses.</summary>
	/// <returns>The <see cref="T:System.Printing.PrintProcessor" /> that the queue uses, such as WinPrint or ModiPrint.</returns>
	public virtual PrintProcessor QueuePrintProcessor
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets a value that represents the status of the printer. These include "warming up," "initializing," "printing," and others.</summary>
	/// <returns>The current <see cref="T:System.Printing.PrintQueueStatus" /> value.</returns>
	public PrintQueueStatus QueueStatus
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the printer prints jobs that have completed the spooling process before jobs that have not fully spooled even if the latter entered the queue first or have a higher priority.</summary>
	/// <returns>
	///   <see langword="true" /> if the printer prints jobs that have completed the spooling process before jobs that have not fully spooled; otherwise, <see langword="false" />.</returns>
	public bool ScheduleCompletedJobsFirst
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets or sets the path and file name of a file that is inserted at the beginning of each print job.</summary>
	/// <returns>The path and file name of the separator file.</returns>
	public virtual string SeparatorFile
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets or sets a name for the printer that is seen by users on the network when it is shared.</summary>
	/// <returns>The public name of a shared printer.</returns>
	public virtual string ShareName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets or sets the earliest time of day, expressed as the number of minutes after midnight Coordinated Universal Time (UTC) (also called Greenwich Mean Time [GMT]), that the printer will print a job.</summary>
	/// <returns>The time of day that the printer first becomes available, expressed as the number of minutes after midnight (UTC). The maximum value is 1439. When a printer is first installed by using the Microsoft Windows Add Printer Wizard, the printer defaults to being available all the time, and this property returns 0 in all time zones.</returns>
	public virtual int StartTimeOfDay
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets or sets the latest time, expressed as the number of minutes after midnight Coordinated Universal Time (UTC) (also called Greenwich Mean Time [GMT]), that the printer will print a job.</summary>
	/// <returns>The time of day that the printer is no longer available, expressed as the number of minutes after midnight (UTC). The maximum value is 1439. When a printer is first installed by using the Microsoft Windows Add Printer Wizard, the printer defaults to being available all the time, and this property returns 0 in all time zones.</returns>
	public virtual int UntilTimeOfDay
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets or sets the current user's default <see cref="T:System.Printing.PrintTicket" /> object, which contains detailed information about the print job.</summary>
	/// <returns>The <see cref="T:System.Printing.PrintTicket" /> for the current user, or <see langword="null" /> if a user <see cref="T:System.Printing.PrintTicket" /> has not been specified.</returns>
	public virtual PrintTicket UserPrintTicket
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal IXpsOMPackageWriter XpsOMPackageWriter
	{
		set
		{
		}
	}

	/// <summary>Initializes a new instance of <see cref="T:System.Printing.PrintQueue" /> class using the specified <see cref="T:System.Printing.PrintServer" /> and queue name.</summary>
	/// <param name="printServer">The print server to host the print queue.</param>
	/// <param name="printQueueName">The name of the print queue.</param>
	public PrintQueue(PrintServer printServer, string printQueueName)
	{
	}

	/// <summary>Initializes a new instance of <see cref="T:System.Printing.PrintQueue" /> class using the specified <see cref="T:System.Printing.PrintServer" />, queue name, and print schema version.</summary>
	/// <param name="printServer">The print server that hosts the print queue.</param>
	/// <param name="printQueueName">The name of the print queue.</param>
	/// <param name="printSchemaVersion">The version of the Print Schema to use.</param>
	public PrintQueue(PrintServer printServer, string printQueueName, int printSchemaVersion)
	{
	}

	/// <summary>Initializes a new instance of <see cref="T:System.Printing.PrintQueue" /> class using the specified <see cref="T:System.Printing.PrintServer" />, queue name, print schema version, and desired access.</summary>
	/// <param name="printServer">The print server that hosts the print queue.</param>
	/// <param name="printQueueName">The name of the print queue.</param>
	/// <param name="printSchemaVersion">The version of the Print Schema to use.</param>
	/// <param name="desiredAccess">One of the <see cref="T:System.Printing.PrintSystemDesiredAccess" /> values that specifies the type of access to the print queue that your program needs.</param>
	/// <exception cref="T:System.Printing.PrintQueueException">
	///   <paramref name="desiredAccess" /> is a value that can be applied only to a <see cref="T:System.Printing.PrintServer" /> object, not a <see cref="T:System.Printing.PrintQueue" /> object. For example, <see cref="F:System.Printing.PrintSystemDesiredAccess.AdministrateServer" />.</exception>
	public PrintQueue(PrintServer printServer, string printQueueName, int printSchemaVersion, PrintSystemDesiredAccess desiredAccess)
	{
	}

	/// <summary>Initializes a new instance of <see cref="T:System.Printing.PrintQueue" /> class using the specified <see cref="T:System.Printing.PrintServer" />, queue name, and array of <see cref="T:System.Printing.PrintQueueIndexedProperty" /> values to initialize.</summary>
	/// <param name="printServer">The print server that hosts the print queue.</param>
	/// <param name="printQueueName">The name of the print queue.</param>
	/// <param name="propertyFilter">An array of <see cref="T:System.Printing.PrintQueueIndexedProperty" /> values that specifies the property values to initialize.</param>
	public PrintQueue(PrintServer printServer, string printQueueName, PrintQueueIndexedProperty[] propertyFilter)
	{
	}

	/// <summary>Initializes a new instance of <see cref="T:System.Printing.PrintQueue" /> class with the specified <see cref="T:System.Printing.PrintServer" />, queue name, array of <see cref="T:System.Printing.PrintQueueIndexedProperty" /> values to initialize, and desired access.</summary>
	/// <param name="printServer">The print server that hosts the print queue.</param>
	/// <param name="printQueueName">The name of the print queue.</param>
	/// <param name="propertyFilter">An array of <see cref="T:System.Printing.PrintQueueIndexedProperty" /> values that specifies the properties to initialize.</param>
	/// <param name="desiredAccess">One of the <see cref="T:System.Printing.PrintSystemDesiredAccess" /> values that specifies the type of access to the print queue that your program needs.</param>
	/// <exception cref="T:System.Printing.PrintQueueException">
	///   <paramref name="desiredAccess" /> is a value that can be applied only to a <see cref="T:System.Printing.PrintServer" /> object, not a <see cref="T:System.Printing.PrintQueue" /> object. For example, <see cref="F:System.Printing.PrintSystemDesiredAccess.AdministrateServer" />.</exception>
	public PrintQueue(PrintServer printServer, string printQueueName, PrintQueueIndexedProperty[] propertyFilter, PrintSystemDesiredAccess desiredAccess)
	{
	}

	/// <summary>Initializes a new instance of <see cref="T:System.Printing.PrintQueue" /> class using the specified <see cref="T:System.Printing.PrintServer" />, queue name, and desired access.</summary>
	/// <param name="printServer">The print server that hosts the print queue.</param>
	/// <param name="printQueueName">The name of the print queue.</param>
	/// <param name="desiredAccess">One of the <see cref="T:System.Printing.PrintSystemDesiredAccess" /> values that specifies the type of access to the print queue that your program needs.</param>
	/// <exception cref="T:System.Printing.PrintQueueException">
	///   <paramref name="desiredAccess" /> is a value that can be applied only to a <see cref="T:System.Printing.PrintServer" /> object, not a <see cref="T:System.Printing.PrintQueue" /> object. For example, <see cref="F:System.Printing.PrintSystemDesiredAccess.AdministrateServer" />.</exception>
	public PrintQueue(PrintServer printServer, string printQueueName, PrintSystemDesiredAccess desiredAccess)
	{
	}

	/// <summary>Initializes a new instance of <see cref="T:System.Printing.PrintQueue" /> class using the specified <see cref="T:System.Printing.PrintServer" />, queue name, and array of property names to initialize.</summary>
	/// <param name="printServer">The print server that hosts the print queue.</param>
	/// <param name="printQueueName">The name of the print queue.</param>
	/// <param name="propertyFilter">An array of the names of properties to initialize.</param>
	public PrintQueue(PrintServer printServer, string printQueueName, string[] propertyFilter)
	{
	}

	/// <summary>Initializes a new instance of <see cref="T:System.Printing.PrintQueue" /> class using the specified <see cref="T:System.Printing.PrintServer" />, queue name, property filter, and desired access.</summary>
	/// <param name="printServer">The print server that hosts the print queue.</param>
	/// <param name="printQueueName">The name of the print queue.</param>
	/// <param name="propertyFilter">An array of the names of properties to initialize.</param>
	/// <param name="desiredAccess">One of the <see cref="T:System.Printing.PrintSystemDesiredAccess" /> values that specifies the type of access to the print queue that your program needs.</param>
	/// <exception cref="T:System.Printing.PrintQueueException">
	///   <paramref name="desiredAccess" /> is a value that can be applied only to a <see cref="T:System.Printing.PrintServer" /> object, not a <see cref="T:System.Printing.PrintQueue" /> object. For example, <see cref="F:System.Printing.PrintSystemDesiredAccess.AdministrateServer" />.</exception>
	public PrintQueue(PrintServer printServer, string printQueueName, string[] propertyFilter, PrintSystemDesiredAccess desiredAccess)
	{
	}

	/// <summary>Inserts a new (generically named) print job, whose content is a <see cref="T:System.Byte" /> array, into the queue.</summary>
	/// <returns>A <see cref="T:System.Printing.PrintSystemJobInfo" /> that represents the print job and its status.</returns>
	public PrintSystemJobInfo AddJob()
	{
		throw null;
	}

	/// <summary>Inserts a new print job, whose content is a <see cref="T:System.Byte" /> array, into the queue.</summary>
	/// <param name="jobName">The name of the print job.</param>
	/// <returns>A <see cref="T:System.Printing.PrintSystemJobInfo" /> that represents the print job and its status.</returns>
	public PrintSystemJobInfo AddJob(string jobName)
	{
		throw null;
	}

	/// <summary>Inserts a new print job for an XML Paper Specification (XPS) Document into the queue, and gives it the specified name and settings.</summary>
	/// <param name="jobName">The path and name of the document that is being printed.</param>
	/// <param name="printTicket">The settings of the print job.</param>
	/// <returns>A <see cref="T:System.Printing.PrintSystemJobInfo" /> that represents the print job and its status.</returns>
	public PrintSystemJobInfo AddJob(string jobName, PrintTicket printTicket)
	{
		throw null;
	}

	/// <summary>Inserts a new print job for an XML Paper Specification (XPS) Document into the queue, gives it the specified name, and specifies whether or not it should be validated.</summary>
	/// <param name="jobName">The name of the print job.</param>
	/// <param name="documentPath">The path and name of the document that is being printed.</param>
	/// <param name="fastCopy">
	///   <see langword="true" /> to spool quickly without page-by-page progress feedback and without validating that the file is valid XPS; otherwise, <see langword="false" />.</param>
	/// <returns>A <see cref="T:System.Printing.PrintSystemJobInfo" /> that represents the print job and its status.</returns>
	public PrintSystemJobInfo AddJob(string jobName, string documentPath, bool fastCopy)
	{
		throw null;
	}

	/// <summary>Inserts a new print job for an XML Paper Specification (XPS) Document into the queue, gives it the specified name and settings, and specifies whether or not it should be validated.</summary>
	/// <param name="jobName">The path and name of the document that is being printed.</param>
	/// <param name="documentPath">The path and name of the document that is being printed.</param>
	/// <param name="fastCopy">
	///   <see langword="true" /> to spool quickly without page-by-page progress feedback and without validating that the file is valid XPS; otherwise, <see langword="false" />.</param>
	/// <param name="printTicket">The settings of the print job.</param>
	/// <returns>A <see cref="T:System.Printing.PrintSystemJobInfo" /> that represents the print job and its status.</returns>
	public PrintSystemJobInfo AddJob(string jobName, string documentPath, bool fastCopy, PrintTicket printTicket)
	{
		throw null;
	}

	/// <summary>Writes the current properties of the <see cref="T:System.Printing.PrintQueue" /> object to the actual print queue on the print server.</summary>
	/// <exception cref="T:System.Printing.PrintSystemException">Some of the properties could not be committed.</exception>
	/// <exception cref="T:System.Printing.PrintCommitAttributesException">Some of the properties could not be committed.  
	///
	/// -or-
	///
	///  The <see cref="T:System.Printing.PrintQueue" /> object was not created with sufficient rights.</exception>
	public override void Commit()
	{
	}

	/// <summary>Creates an <see cref="T:System.Windows.Xps.XpsDocumentWriter" /> object with the specified dimensions.</summary>
	/// <param name="width">The width of the XPS document.</param>
	/// <param name="height">The height of the XPS document.</param>
	/// <returns>An <see cref="T:System.Windows.Xps.XpsDocumentWriter" /> that writes to an XPS stream. This can be <see langword="null" />.</returns>
	public static XpsDocumentWriter CreateXpsDocumentWriter(ref double width, ref double height)
	{
		throw null;
	}

	/// <summary>Creates an <see cref="T:System.Windows.Xps.XpsDocumentWriter" /> object, opens a Windows common print dialog and returns a <see langword="ref" /> (<see langword="ByRef" /> in Visual Basic) parameter that represents information about the imageable area and the dimensions of the media.</summary>
	/// <param name="documentImageableArea">A reference to an object that contains the dimensions of the area of the page on which the device can print. Since its data type has no public constructor, this parameter is passed uninitialized.</param>
	/// <returns>An <see cref="T:System.Windows.Xps.XpsDocumentWriter" /> that writes XPS data to a stream. This can be <see langword="null" />. (The parameter <paramref name="documentImageableArea" /> is a <see langword="ref" /> [<see langword="ByRef" /> in Visual Basic] parameter that is initialized by the method, so it represents a second returned item.)</returns>
	public static XpsDocumentWriter CreateXpsDocumentWriter(ref PrintDocumentImageableArea documentImageableArea)
	{
		throw null;
	}

	/// <summary>Creates an <see cref="T:System.Windows.Xps.XpsDocumentWriter" /> object, opens a Windows common print dialog, provides the dialog with a page range and a description of the print job, and returns a <see langword="ref" /> (<see langword="ByRef" /> in Visual Basic) parameter that represents information about the imageable area and the dimensions of the media.</summary>
	/// <param name="documentImageableArea">A reference to an object that contains the dimensions of the area of the page on which the device can print. Since its data type has no public constructor, this parameter is passed uninitialized.</param>
	/// <param name="pageRangeSelection">A value that specifies whether to print all pages or only a range that is specified by the user.</param>
	/// <param name="pageRange">The range of pages that is printed.</param>
	/// <returns>An <see cref="T:System.Windows.Xps.XpsDocumentWriter" /> that writes XPS data to a stream. This can be <see langword="null" />. (The parameters <paramref name="documentImageableArea" />, <paramref name="pageRangeSelection" />, and <paramref name="pageRange" /> are all <see langword="ref" /> [<see langword="ByRef" /> in Visual Basic] parameters that are initialized by the user and returned when the dialog is closed, so each represents an additional returned item.)</returns>
	public static XpsDocumentWriter CreateXpsDocumentWriter(ref PrintDocumentImageableArea documentImageableArea, ref PageRangeSelection pageRangeSelection, ref PageRange pageRange)
	{
		throw null;
	}

	/// <summary>Creates an <see cref="T:System.Windows.Xps.XpsDocumentWriter" /> object and associates it with the specified print queue.</summary>
	/// <param name="printQueue">A print queue to print the XPS document.</param>
	/// <returns>An <see cref="T:System.Windows.Xps.XpsDocumentWriter" /> that writes to an XPS stream.</returns>
	public static XpsDocumentWriter CreateXpsDocumentWriter(PrintQueue printQueue)
	{
		throw null;
	}

	/// <summary>Creates an <see cref="T:System.Windows.Xps.XpsDocumentWriter" /> object, opens a Windows common print dialog (and provides it a job description) and returns a <see langword="ref" /> (<see langword="ByRef" /> in Visual Basic) parameter that represents information about the imageable area and the dimensions of the media.</summary>
	/// <param name="jobDescription">A name for the print job. It appears in the Windows printing user interface.</param>
	/// <param name="documentImageableArea">A reference to an object that contains the dimensions of the area of the page on which the device can print. Since its data type has no public constructor, this parameter is passed uninitialized.</param>
	/// <returns>An <see cref="T:System.Windows.Xps.XpsDocumentWriter" /> that writes XPS data to a stream. This can be <see langword="null" />. (The parameter <paramref name="documentImageableArea" /> is a <see langword="ref" /> [<see langword="ByRef" /> in Visual Basic] parameter that is initialized by the method, so it represents a second returned item.)</returns>
	public static XpsDocumentWriter CreateXpsDocumentWriter(string jobDescription, ref PrintDocumentImageableArea documentImageableArea)
	{
		throw null;
	}

	/// <summary>Creates an <see cref="T:System.Windows.Xps.XpsDocumentWriter" /> object, opens a Windows common print dialog, provides the dialog with a page range, and returns a <see langword="ref" /> (<see langword="ByRef" /> in Visual Basic) parameter that represents information about the imageable area and the dimensions of the media.</summary>
	/// <param name="jobDescription">A name for the print job. It appears in the Windows printing user interface.</param>
	/// <param name="documentImageableArea">A reference to an object that contains the dimensions of the area of the page on which the device can print. Since its data type has no public constructor, this parameter is passed uninitialized.</param>
	/// <param name="pageRangeSelection">A value that specifies whether to print all pages or only a range that is specified by the user.</param>
	/// <param name="pageRange">The range of pages that is printed.</param>
	/// <returns>An <see cref="T:System.Windows.Xps.XpsDocumentWriter" /> that writes XPS data to a stream. This can be <see langword="null" />. (The parameters <paramref name="documentImageableArea" />, <paramref name="pageRangeSelection" />, and <paramref name="pageRange" /> are all <see langword="ref" /> [<see langword="ByRef" /> in Visual Basic] parameters that are initialized by the user and returned when the dialog is closed, so each represents an additional returned item.)</returns>
	public static XpsDocumentWriter CreateXpsDocumentWriter(string jobDescription, ref PrintDocumentImageableArea documentImageableArea, ref PageRangeSelection pageRangeSelection, ref PageRange pageRange)
	{
		throw null;
	}

	/// <summary>Gets the print job with the specified ID number.</summary>
	/// <param name="jobId">The number of the job in the queue.</param>
	/// <returns>A <see cref="T:System.Printing.PrintSystemJobInfo" /> that specifies the properties of the job and its status.</returns>
	public PrintSystemJobInfo GetJob(int jobId)
	{
		throw null;
	}

	/// <summary>Gets a <see cref="T:System.Printing.PrintCapabilities" /> object that identifies the capabilities of the printer.</summary>
	/// <exception cref="T:System.Printing.PrintQueueException">The <see cref="T:System.Printing.PrintCapabilities" /> object could not be retrieved.</exception>
	/// <returns>A <see cref="T:System.Printing.PrintCapabilities" /> object that specifies what the printer can and cannot do, such as two-sided coping or automatic stapling.</returns>
	public PrintCapabilities GetPrintCapabilities()
	{
		throw null;
	}

	/// <summary>Gets a <see cref="T:System.Printing.PrintCapabilities" /> object that identifies the capabilities of the printer.</summary>
	/// <param name="printTicket">A print ticket that provides the basis on which the print capabilities are reported.</param>
	/// <exception cref="T:System.Printing.PrintQueueException">The <see cref="T:System.Printing.PrintCapabilities" /> object could not be retrieved.</exception>
	/// <exception cref="T:System.ArgumentException">
	///   <paramref name="printTicket" /> is not well-formed.</exception>
	/// <returns>A <see cref="T:System.Printing.PrintCapabilities" /> object that specifies what the printer can and cannot do, such as two-sided coping or automatic stapling.</returns>
	public PrintCapabilities GetPrintCapabilities(PrintTicket printTicket)
	{
		throw null;
	}

	/// <summary>Gets a <see cref="T:System.IO.MemoryStream" /> object that specifies the printer's capabilities as an XML stream that complies with the Print Schema.</summary>
	/// <exception cref="T:System.Printing.PrintQueueException">The print capabilities could not be retrieved.</exception>
	/// <returns>A <see cref="T:System.IO.MemoryStream" /> specifying the printer's capabilities by using the XML schema "PrintCapabilities," a part of the Print Schema system.</returns>
	public MemoryStream GetPrintCapabilitiesAsXml()
	{
		throw null;
	}

	/// <summary>Gets a <see cref="T:System.IO.MemoryStream" /> object that specifies the printer's capabilities in an XML format that complies with the Print Schema.</summary>
	/// <param name="printTicket">A print ticket that provides the basis on which the print capabilities are reported.</param>
	/// <exception cref="T:System.Printing.PrintQueueException">The print capabilities could not be retrieved.</exception>
	/// <exception cref="T:System.ArgumentException">
	///   <paramref name="printTicket" /> is not well-formed.</exception>
	/// <returns>A <see cref="T:System.IO.MemoryStream" /> specifying the printer's capabilities by using the XML schema "PrintCapabilities," a part of the Print Schema system.</returns>
	public MemoryStream GetPrintCapabilitiesAsXml(PrintTicket printTicket)
	{
		throw null;
	}

	/// <summary>Creates a collection that contains a <see cref="T:System.Printing.PrintSystemJobInfo" /> object for each job in the queue.</summary>
	/// <returns>A collection of <see cref="T:System.Printing.PrintSystemJobInfo" /> objects. There is one for each job in the queue.</returns>
	public PrintJobInfoCollection GetPrintJobInfoCollection()
	{
		throw null;
	}

	/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Printing.PrintQueue" /> and optionally releases the managed resources.</summary>
	/// <param name="disposing">
	///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
	protected sealed override void InternalDispose(bool disposing)
	{
	}

	/// <summary>Merges two <see cref="T:System.Printing.PrintTicket" />s and guarantees that the resulting <see cref="T:System.Printing.PrintTicket" /> is valid and does not ask for any printing functionality that the printer does not support.</summary>
	/// <param name="basePrintTicket">The first print ticket.</param>
	/// <param name="deltaPrintTicket">The second print ticket. This can be <see langword="null" />.</param>
	/// <exception cref="T:System.ArgumentException">At least one of the input print tickets is not valid.</exception>
	/// <exception cref="T:System.ArgumentNullException">The <paramref name="basePrintTicket" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.Printing.PrintQueueException">The validation, merger, and viability checking operation failed.</exception>
	/// <returns>A <see cref="T:System.Printing.ValidationResult" /> that includes the merged <see cref="T:System.Printing.PrintTicket" /> and an indication of whether any of its settings had to be changed to guarantee viability.</returns>
	public ValidationResult MergeAndValidatePrintTicket(PrintTicket basePrintTicket, PrintTicket deltaPrintTicket)
	{
		throw null;
	}

	/// <summary>Merges two <see cref="T:System.Printing.PrintTicket" />s and guarantees that the resulting <see cref="T:System.Printing.PrintTicket" /> is valid, does not ask for any printing functionality that the printer does not support, and is limited to the specified scope.</summary>
	/// <param name="basePrintTicket">The first print ticket.</param>
	/// <param name="deltaPrintTicket">The second print ticket. This can be <see langword="null" />.</param>
	/// <param name="scope">A value indicating whether the scope of <paramref name="deltaPrintTicket" />, and the scope of the print ticket returned in the <see cref="T:System.Printing.ValidationResult" />, is a page, a document, or the whole job.</param>
	/// <exception cref="T:System.ArgumentException">At least one of the input print tickets is not valid.</exception>
	/// <exception cref="T:System.ArgumentNullException">The <paramref name="basePrintTicket" /> is <see langword="null" />.</exception>
	/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="scope" /> parameter does not have a valid <see cref="T:System.Printing.PrintTicketScope" /> value.</exception>
	/// <exception cref="T:System.Printing.PrintQueueException">The validation, merger, and viability checking operation failed.</exception>
	/// <returns>A <see cref="T:System.Printing.ValidationResult" /> that includes the merged <see cref="T:System.Printing.PrintTicket" /> and an indication of whether any of its settings had to be changed to guarantee viability.</returns>
	public ValidationResult MergeAndValidatePrintTicket(PrintTicket basePrintTicket, PrintTicket deltaPrintTicket, PrintTicketScope scope)
	{
		throw null;
	}

	/// <summary>Pauses the print queue. It remains paused until <see cref="M:System.Printing.PrintQueue.Resume" /> is executed.</summary>
	/// <exception cref="T:System.Printing.PrintSystemException">The printer cannot be paused.</exception>
	public virtual void Pause()
	{
	}

	/// <summary>Removes all the jobs in the print queue.</summary>
	/// <exception cref="T:System.Printing.PrintSystemException">Some print jobs could not be removed from the queue.</exception>
	public virtual void Purge()
	{
	}

	/// <summary>Updates the properties of the <see cref="T:System.Printing.PrintQueue" /> object with values from the printer and the print queue utility that runs on the computer.</summary>
	/// <exception cref="T:System.Printing.PrintSystemException">Some of the properties could not be refreshed.</exception>
	public override void Refresh()
	{
	}

	/// <summary>Restarts a print queue that was paused.</summary>
	/// <exception cref="T:System.Printing.PrintSystemException">The printer cannot resume.</exception>
	public virtual void Resume()
	{
	}

	internal static uint GetDpiX(ILegacyDevice legacyDevice)
	{
		throw null;
	}

	internal static uint GetDpiY(ILegacyDevice legacyDevice)
	{
		throw null;
	}

	internal ILegacyDevice GetLegacyDevice()
	{
		throw null;
	}
}
