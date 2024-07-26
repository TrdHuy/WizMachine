namespace System.Printing;

/// <summary>Represents the local print server (the computer on which your application is running) and enables management of its print queues.</summary>
public sealed class LocalPrintServer : PrintServer
{
	/// <summary>Gets or sets the default print queue.</summary>
	/// <returns>The <see cref="T:System.Printing.PrintQueue" /> that is designated as the default queue for the local computer.</returns>
	public PrintQueue DefaultPrintQueue
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.LocalPrintServer" /> class.</summary>
	public LocalPrintServer()
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.LocalPrintServer" /> class that has the specified <see cref="T:System.Printing.LocalPrintServerIndexedProperty" /> array.</summary>
	/// <param name="propertiesFilter">An array of properties that the constructor initializes.</param>
	public LocalPrintServer(LocalPrintServerIndexedProperty[] propertiesFilter)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.LocalPrintServer" /> class that has the specified <see cref="T:System.Printing.LocalPrintServerIndexedProperty" /> array and the specified <see cref="T:System.Printing.PrintSystemDesiredAccess" />.</summary>
	/// <param name="propertiesFilter">An array of properties that the constructor initializes.</param>
	/// <param name="desiredAccess">A value specifying the type of access to the print server that your program needs.</param>
	/// <exception cref="T:System.Printing.PrintServerException">
	///   <paramref name="desiredAccess" /> is a value that can be applied only to a <see cref="T:System.Printing.PrintQueue" /> object, not a <see cref="T:System.Printing.LocalPrintServer" /> object. For example, <see cref="F:System.Printing.PrintSystemDesiredAccess.UsePrinter" />.</exception>
	public LocalPrintServer(LocalPrintServerIndexedProperty[] propertiesFilter, PrintSystemDesiredAccess desiredAccess)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.LocalPrintServer" /> class that has the specified <see cref="T:System.Printing.PrintSystemDesiredAccess" />.</summary>
	/// <param name="desiredAccess">A value specifying the type of access to the print server that your application needs.</param>
	/// <exception cref="T:System.Printing.PrintServerException">
	///   <paramref name="desiredAccess" /> is a value that can be applied only to a <see cref="T:System.Printing.PrintQueue" /> object, not a <see cref="T:System.Printing.LocalPrintServer" /> object. For example, <see cref="F:System.Printing.PrintSystemDesiredAccess.UsePrinter" />.</exception>
	public LocalPrintServer(PrintSystemDesiredAccess desiredAccess)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.LocalPrintServer" /> class that has the specified properties.</summary>
	/// <param name="propertiesFilter">An array of the property names that the constructor initializes.</param>
	public LocalPrintServer(string[] propertiesFilter)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.LocalPrintServer" /> class that has the specified properties and <see cref="T:System.Printing.PrintSystemDesiredAccess" />.</summary>
	/// <param name="propertiesFilter">An array of the property names that the constructor initializes.</param>
	/// <param name="desiredAccess">A value specifying the type of access to the print server that your application needs.</param>
	/// <exception cref="T:System.Printing.PrintServerException">
	///   <paramref name="desiredAccess" /> is a value that can be applied only to a <see cref="T:System.Printing.PrintQueue" /> object, not a <see cref="T:System.Printing.LocalPrintServer" /> object. For example, <see cref="F:System.Printing.PrintSystemDesiredAccess.UsePrinter" />.</exception>
	public LocalPrintServer(string[] propertiesFilter, PrintSystemDesiredAccess desiredAccess)
	{
	}

	/// <summary>Writes any changes that your application made to the properties of the <see cref="T:System.Printing.LocalPrintServer" /> to the actual print server that the object represents.</summary>
	/// <exception cref="T:System.Printing.PrintServerException">Some properties are not committed.</exception>
	/// <exception cref="T:System.Printing.PrintCommitAttributesException">Some of the properties could not be committed.  
	///
	/// -or-
	///
	///  The <see cref="T:System.Printing.LocalPrintServer" /> object was not created with sufficient rights.</exception>
	public sealed override void Commit()
	{
	}

	/// <summary>Connects the local print server to the specified <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="printer">The print queue to connect.</param>
	/// <exception cref="T:System.Printing.PrintServerException">A print queue that matches the <see cref="P:System.Printing.PrintQueue.FullName" /> property of the <paramref name="printer" /> is not found.</exception>
	/// <returns>
	///   <see langword="true" /> if the connection is made; otherwise <see langword="false" />.</returns>
	public bool ConnectToPrintQueue(PrintQueue printer)
	{
		throw null;
	}

	/// <summary>Connects to the print queue that is specified by using the <see cref="T:System.String" />.</summary>
	/// <param name="printQueuePath">The full path of the print queue that is being connected.</param>
	/// <exception cref="T:System.Printing.PrintServerException">A print queue with the specified path is not found.</exception>
	/// <returns>
	///   <see langword="true" /> if the connection is made; otherwise <see langword="false" />.</returns>
	public bool ConnectToPrintQueue(string printQueuePath)
	{
		throw null;
	}

	/// <summary>Disconnects the local print server from the specified <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="printer">The print queue that is being disconnected.</param>
	/// <exception cref="T:System.Printing.PrintServerException">A print queue matching the <see cref="P:System.Printing.PrintQueue.FullName" /> property of the <paramref name="printer" /> is not found.</exception>
	/// <returns>
	///   <see langword="true" /> if the disconnection is successful; otherwise <see langword="false" />.</returns>
	public bool DisconnectFromPrintQueue(PrintQueue printer)
	{
		throw null;
	}

	/// <summary>Disconnects from the print queue that is specified in the <see cref="T:System.String" />.</summary>
	/// <param name="printQueuePath">The full path to the print queue that is disconnected.</param>
	/// <exception cref="T:System.Printing.PrintServerException">A print queue with the specified path is not found.</exception>
	/// <returns>
	///   <see langword="true" /> if the disconnection is successful; otherwise <see langword="false" />.</returns>
	public bool DisconnectFromPrintQueue(string printQueuePath)
	{
		throw null;
	}

	/// <summary>Returns a reference to the default print queue of the <see cref="T:System.Printing.LocalPrintServer" />.</summary>
	/// <returns>The default <see cref="T:System.Printing.PrintQueue" />.</returns>
	public static PrintQueue GetDefaultPrintQueue()
	{
		throw null;
	}

	/// <summary>Updates the properties of the <see cref="T:System.Printing.LocalPrintServer" /> object so that their values match the values of the print server that the object represents.</summary>
	/// <exception cref="T:System.Printing.PrintServerException">Some properties did not update.</exception>
	public sealed override void Refresh()
	{
	}
}
