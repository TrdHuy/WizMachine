using System.Printing.IndexedProperties;
using System.Threading;

namespace System.Printing;

/// <summary>Manages the print queues on a print server, which is usually a computer, but can be a dedicated hardware print server appliance.</summary>
public class PrintServer : PrintSystemObject
{
	/// <summary>Gets or sets a value that indicates whether the print server beeps in response to an error condition in the printer.</summary>
	/// <returns>
	///   <see cref="T:System.Boolean" />
	///   <see langword="true" /> if the print server beeps in response to an error; otherwise, <see langword="false" />.</returns>
	public bool BeepEnabled
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Do not use.</summary>
	/// <returns>The thread priority.</returns>
	public ThreadPriority DefaultPortThreadPriority
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Do not use.</summary>
	/// <returns>The default scheduler thread priority.</returns>
	public ThreadPriority DefaultSchedulerPriority
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets or sets the path where the print server's spool files are located.</summary>
	/// <returns>A <see cref="T:System.String" /> that identifies the complete path of the folder for the spool files.</returns>
	public string DefaultSpoolDirectory
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets or sets the type of events that the print server logs.</summary>
	/// <returns>A value of <see cref="T:System.Printing.PrintServerEventLoggingTypes" /> that identifies the type of event logging that is provided by the print server. The default is <see cref="F:System.Printing.PrintServerEventLoggingTypes.LogPrintingErrorEvents" />.</returns>
	public PrintServerEventLoggingTypes EventLog
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets or sets a value that indicates whether initialization of the <see cref="T:System.Printing.PrintServer" /> properties has been postponed.</summary>
	/// <returns>
	///   <see langword="true" /> if initialization is postponed; otherwise, <see langword="false" />.</returns>
	protected bool IsDelayInitialized
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets the major version of the operating system.</summary>
	/// <returns>An <see cref="T:System.Int32" /> that identifies the major version of the operating system.</returns>
	public int MajorVersion
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets the minor version within the major version of the operating system.</summary>
	/// <returns>An <see cref="T:System.Int32" /> that identifies the minor version of the operating system.</returns>
	public int MinorVersion
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets the name of the print server.</summary>
	/// <exception cref="T:System.Printing.PrintSystemException">The property is not initialized.</exception>
	/// <returns>The name of the print server.</returns>
	public sealed override string Name
	{
		get
		{
			throw null;
		}
		internal set
		{
			base.Name = value;
		}
	}

	/// <summary>Gets or sets a value that indicates whether notifications that a print job has finished are sent to either the print server or the client computer.</summary>
	/// <returns>
	///   <see langword="true" /> if notifications are sent to the client computer; <see langword="false" /> if notifications are sent to the print server.</returns>
	public bool NetPopup
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets or sets the thread priority for the process that manages I/O through the printer ports.</summary>
	/// <returns>A <see cref="T:System.Threading.ThreadPriority" /> enumeration value that identifies the thread priority for the process that manages the printer ports. The default is <see cref="F:System.Threading.ThreadPriority.Normal" />.</returns>
	public ThreadPriority PortThreadPriority
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets or sets a value that indicates whether users can restart jobs after an error occurs if printer pooling is enabled.</summary>
	/// <returns>
	///   <see langword="true" /> if jobs can be restarted when printer pooling is enabled; otherwise, <see langword="false" />.</returns>
	public bool RestartJobOnPoolEnabled
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets or sets a value that indicates the wait time before a job can be restarted, if an error occurs when printer pooling is also enabled.</summary>
	/// <returns>The wait time, in minutes, before a job restarts.</returns>
	public int RestartJobOnPoolTimeout
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets or sets the thread priority for the process that routes print jobs from applications to print queues.</summary>
	/// <returns>A <see cref="T:System.Threading.ThreadPriority" /> enumeration value that identifies the thread priority for the print server scheduling process. The default is <see cref="F:System.Threading.ThreadPriority.Normal" />.</returns>
	public ThreadPriority SchedulerPriority
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets the version of the print spooler system.</summary>
	/// <returns>A <see cref="T:System.Byte" /> that identifies the version of the print spooler system.</returns>
	public byte SubSystemVersion
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.PrintServer" /> class.</summary>
	public PrintServer()
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.PrintServer" /> class that represents the local print server and assigns it the specified <see cref="T:System.Printing.PrintSystemDesiredAccess" />.</summary>
	/// <param name="desiredAccess">A value that specifies the type of print server access that your program needs.</param>
	/// <exception cref="T:System.Printing.PrintServerException">
	///   <paramref name="desiredAccess" /> is a value that can be applied only to a <see cref="T:System.Printing.PrintQueue" /> object, not a <see cref="T:System.Printing.LocalPrintServer" /> object. For example, <see cref="F:System.Printing.PrintSystemDesiredAccess.UsePrinter" />.</exception>
	public PrintServer(PrintSystemDesiredAccess desiredAccess)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.PrintServer" /> class that has the specified path.</summary>
	/// <param name="path">The name and complete path of the print server.</param>
	public PrintServer(string path)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.PrintServer" /> class by using the specified <see cref="T:System.Printing.PrintServerIndexedProperty" /> array to determine which properties will be initialized.</summary>
	/// <param name="path">The complete path and name of the print server.</param>
	/// <param name="propertiesFilter">The properties that the constructor initializes.</param>
	public PrintServer(string path, PrintServerIndexedProperty[] propertiesFilter)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.PrintServer" /> class and provides the specified path, the <see cref="T:System.Printing.PrintServerIndexedProperty" /> array, and the needed access.</summary>
	/// <param name="path">The complete path and name of the print server.</param>
	/// <param name="propertiesFilter">The properties that the constructor initializes.</param>
	/// <param name="desiredAccess">A value that specifies the type of print server access that your program needs.</param>
	/// <exception cref="T:System.Printing.PrintServerException">
	///   <paramref name="desiredAccess" /> is a value that can be applied only to a <see cref="T:System.Printing.PrintQueue" /> object, not a <see cref="T:System.Printing.LocalPrintServer" /> object. For example, <see cref="F:System.Printing.PrintSystemDesiredAccess.UsePrinter" />.</exception>
	public PrintServer(string path, PrintServerIndexedProperty[] propertiesFilter, PrintSystemDesiredAccess desiredAccess)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.PrintServer" /> class that has the specified path and the needed access.</summary>
	/// <param name="path">The name and complete path of the print server.</param>
	/// <param name="desiredAccess">A value that specifies the type of print server access that your program needs.</param>
	/// <exception cref="T:System.Printing.PrintServerException">
	///   <paramref name="desiredAccess" /> is a value that can be applied only to a <see cref="T:System.Printing.PrintQueue" /> object, not a <see cref="T:System.Printing.LocalPrintServer" /> object. For example, <see cref="F:System.Printing.PrintSystemDesiredAccess.UsePrinter" />.</exception>
	public PrintServer(string path, PrintSystemDesiredAccess desiredAccess)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.PrintServer" /> class that has the specified path and properties filter.</summary>
	/// <param name="path">The name and complete path of the print server.</param>
	/// <param name="propertiesFilter">An array of the names of properties that the constructor initializes.</param>
	public PrintServer(string path, string[] propertiesFilter)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.PrintServer" /> class that has the specified path, properties filter, and the needed access.</summary>
	/// <param name="path">The name and complete path of the print server.</param>
	/// <param name="propertiesFilter">An array of the names of properties that the constructor initializes.</param>
	/// <param name="desiredAccess">A value that specifies the type of print server access that your program needs.</param>
	/// <exception cref="T:System.Printing.PrintServerException">
	///   <paramref name="desiredAccess" /> is a value that can be applied only to a <see cref="T:System.Printing.PrintQueue" /> object, not a <see cref="T:System.Printing.LocalPrintServer" /> object. For example, <see cref="F:System.Printing.PrintSystemDesiredAccess.UsePrinter" />.</exception>
	public PrintServer(string path, string[] propertiesFilter, PrintSystemDesiredAccess desiredAccess)
	{
	}

	/// <summary>Commits any changes that your program made to the properties of the print server object by writing them to the print server that the object represents.</summary>
	/// <exception cref="T:System.Printing.PrintSystemException">Some of the properties are not committed.</exception>
	/// <exception cref="T:System.Printing.PrintCommitAttributesException">Some of the properties could not be committed.  
	///
	/// -or-
	///
	///  The <see cref="T:System.Printing.PrintServer" /> object was not created with sufficient rights.</exception>
	public override void Commit()
	{
	}

	/// <summary>Removes the specified <see cref="T:System.Printing.PrintQueue" /> from the print server.</summary>
	/// <param name="printQueue">The queue that is deleted.</param>
	/// <returns>
	///   <see langword="true" /> if the queue is successfully deleted; otherwise, <see langword="false" />.</returns>
	public static bool DeletePrintQueue(PrintQueue printQueue)
	{
		throw null;
	}

	/// <summary>Removes the print queue with the specified name from the print server.</summary>
	/// <param name="printQueueName">The name of the queue that is deleted.</param>
	/// <returns>
	///   <see langword="true" /> if the queue is successfully deleted; otherwise, <see langword="false" />.</returns>
	public static bool DeletePrintQueue(string printQueueName)
	{
		throw null;
	}

	/// <summary>Obtains a reference to the named print queue from the print server.</summary>
	/// <param name="printQueueName">The name of the print queue.</param>
	/// <returns>The <see cref="T:System.Printing.PrintQueue" /> with the specified name from the print server.</returns>
	public PrintQueue GetPrintQueue(string printQueueName)
	{
		throw null;
	}

	/// <summary>Gets a specified print queue from the print server.</summary>
	/// <param name="printQueueName">The name of the print queue.</param>
	/// <param name="propertiesFilter">The names of the properties that are initialized in the returned queue.</param>
	/// <returns>The <see cref="T:System.Printing.PrintQueue" /> with the specified name from the print server.</returns>
	public PrintQueue GetPrintQueue(string printQueueName, string[] propertiesFilter)
	{
		throw null;
	}

	/// <summary>Gets the collection of print queues that the print server hosts.</summary>
	/// <returns>The <see cref="T:System.Printing.PrintQueueCollection" /> of print queues on the print server.</returns>
	public PrintQueueCollection GetPrintQueues()
	{
		throw null;
	}

	/// <summary>Gets the collection of print queues of the specified types that are named in <see cref="T:System.Printing.EnumeratedPrintQueueTypes" /> and hosted by the print server.</summary>
	/// <param name="enumerationFlag">An array of values that represent the types of print queues that are in the collection.</param>
	/// <returns>The <see cref="T:System.Printing.PrintQueueCollection" /> of print queues, of the specified types, on the print server.</returns>
	public PrintQueueCollection GetPrintQueues(EnumeratedPrintQueueTypes[] enumerationFlag)
	{
		throw null;
	}

	/// <summary>Gets a collection of print queues that are hosted by the print server and initialized only in the properties that are specified in the <see cref="T:System.Printing.PrintQueueIndexedProperty" /> array.</summary>
	/// <param name="propertiesFilter">The properties that the constructor initializes.</param>
	/// <returns>A <see cref="T:System.Printing.PrintQueueCollection" /> whose members are initialized only in the properties specified by <paramref name="propertiesFilter" />.</returns>
	public PrintQueueCollection GetPrintQueues(PrintQueueIndexedProperty[] propertiesFilter)
	{
		throw null;
	}

	/// <summary>Gets a collection of print queues of the specified types. These print queues are only initialized in the properties that are specified in the <see cref="T:System.Printing.PrintQueueIndexedProperty" /> array.</summary>
	/// <param name="propertiesFilter">The properties that the constructor initializes.</param>
	/// <param name="enumerationFlag">An array of values that represent the types of print queues in the collection.</param>
	/// <returns>The <see cref="T:System.Printing.PrintQueueCollection" /> of the print server.</returns>
	public PrintQueueCollection GetPrintQueues(PrintQueueIndexedProperty[] propertiesFilter, EnumeratedPrintQueueTypes[] enumerationFlag)
	{
		throw null;
	}

	/// <summary>Gets a collection of print queues that are hosted by the print server and that are initialized only in the specified properties.</summary>
	/// <param name="propertiesFilter">The names of the queue properties that are initialized.</param>
	/// <returns>The <see cref="T:System.Printing.PrintQueueCollection" /> of print queues on the print server; each print queue is initialized only in the properties that are specified in <paramref name="propertiesFilter" />.</returns>
	public PrintQueueCollection GetPrintQueues(string[] propertiesFilter)
	{
		throw null;
	}

	/// <summary>Gets the collection of print queues, which are of the specified <see cref="T:System.Printing.EnumeratedPrintQueueTypes" /> and are initialized only in the specified properties.</summary>
	/// <param name="propertiesFilter">The names of the queue properties that are initialized.</param>
	/// <param name="enumerationFlag">An array of values that represent the types of print queues that are returned in the collection.</param>
	/// <returns>A <see cref="T:System.Printing.PrintQueueCollection" /> of print queues of the specified types; each print queue has only the specified properties initialized.</returns>
	public PrintQueueCollection GetPrintQueues(string[] propertiesFilter, EnumeratedPrintQueueTypes[] enumerationFlag)
	{
		throw null;
	}

	/// <summary>Installs a print queue, and associated printer driver, on the print server.</summary>
	/// <param name="printQueueName">The name of the new queue.</param>
	/// <param name="driverName">The path and name of the printer driver.</param>
	/// <param name="portNames">The IDs of the ports that the new queue uses.</param>
	/// <param name="printProcessorName">The name of the print processor.</param>
	/// <param name="initialParameters">The parameters that are initialized.</param>
	/// <returns>The new <see cref="T:System.Printing.PrintQueue" />.</returns>
	public PrintQueue InstallPrintQueue(string printQueueName, string driverName, string[] portNames, string printProcessorName, PrintPropertyDictionary initialParameters)
	{
		throw null;
	}

	/// <summary>Installs a print queue, and associated printer driver, on the print server.</summary>
	/// <param name="printQueueName">The name of the new queue.</param>
	/// <param name="driverName">The path and name of the printer driver.</param>
	/// <param name="portNames">The IDs of the ports that the new queue uses.</param>
	/// <param name="printProcessorName">The name of the print processor.</param>
	/// <param name="printQueueAttributes">The attributes, as flags, of the new queue.</param>
	/// <returns>The newly created <see cref="T:System.Printing.PrintQueue" />.</returns>
	public PrintQueue InstallPrintQueue(string printQueueName, string driverName, string[] portNames, string printProcessorName, PrintQueueAttributes printQueueAttributes)
	{
		throw null;
	}

	/// <summary>Installs a prioritized print queue, and associated printer driver, on the print server.</summary>
	/// <param name="printQueueName">The name of the new queue.</param>
	/// <param name="driverName">The path and name of the printer driver.</param>
	/// <param name="portNames">The IDs of the ports that the new queue uses.</param>
	/// <param name="printProcessorName">The name of the print processor.</param>
	/// <param name="printQueueAttributes">The attributes, as flags, of the new queue.</param>
	/// <param name="printQueueProperty">The comment, location, or share name of the new queue.</param>
	/// <param name="printQueuePriority">A value from 1 through 99 that specifies the priority of this print queue relative to other queues that are hosted by the print server.</param>
	/// <param name="printQueueDefaultPriority">A value from 1 to 99 that specifies the default priority of print jobs that are sent to the queue.</param>
	/// <returns>The newly created <see cref="T:System.Printing.PrintQueue" />.</returns>
	public PrintQueue InstallPrintQueue(string printQueueName, string driverName, string[] portNames, string printProcessorName, PrintQueueAttributes printQueueAttributes, PrintQueueStringProperty printQueueProperty, int printQueuePriority, int printQueueDefaultPriority)
	{
		throw null;
	}

	/// <summary>Installs a shared, prioritized print queue, and associated printer driver, on the print server.</summary>
	/// <param name="printQueueName">The name of the new queue.</param>
	/// <param name="driverName">The path and name of the printer driver.</param>
	/// <param name="portNames">The IDs of the ports that the new queue uses.</param>
	/// <param name="printProcessorName">The name of the print processor.</param>
	/// <param name="printQueueAttributes">The attributes, as flags, of the new queue.</param>
	/// <param name="printQueueShareName">The share name of the new queue.</param>
	/// <param name="printQueueComment">A comment about the queue that is visible to users in the Microsoft Windows UI.</param>
	/// <param name="printQueueLocation">The location of the new queue.</param>
	/// <param name="printQueueSeparatorFile">The path of a file that is inserted at the beginning of each print job.</param>
	/// <param name="printQueuePriority">A value from 1 through 99 that specifies the priority of the queue relative to other queues that are hosted by the print server.</param>
	/// <param name="printQueueDefaultPriority">A value from 1 through 99 that specifies the default priority of new print jobs that are sent to the queue.</param>
	/// <returns>The newly created <see cref="T:System.Printing.PrintQueue" />.</returns>
	public PrintQueue InstallPrintQueue(string printQueueName, string driverName, string[] portNames, string printProcessorName, PrintQueueAttributes printQueueAttributes, string printQueueShareName, string printQueueComment, string printQueueLocation, string printQueueSeparatorFile, int printQueuePriority, int printQueueDefaultPriority)
	{
		throw null;
	}

	/// <summary>Releases the unmanaged resources that are used by the <see cref="T:System.Printing.PrintServer" /> and optionally releases the managed resources.</summary>
	/// <param name="disposing">
	///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
	protected sealed override void InternalDispose(bool disposing)
	{
	}

	/// <summary>Updates the properties of the <see cref="T:System.Printing.PrintServer" /> object so that each property value matches the corresponding attribute value of the print server that the object represents.</summary>
	/// <exception cref="T:System.Printing.PrintSystemException">Some of the properties cannot be refreshed.</exception>
	public override void Refresh()
	{
	}
}
