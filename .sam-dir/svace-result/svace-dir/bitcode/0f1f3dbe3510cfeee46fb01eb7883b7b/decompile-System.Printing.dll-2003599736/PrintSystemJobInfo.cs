using System.IO;

namespace System.Printing;

/// <summary>Defines a print job in detail.</summary>
public class PrintSystemJobInfo : PrintSystemObject
{
	/// <summary>Gets the print queue that is hosting the print job.</summary>
	/// <returns>A <see cref="T:System.Printing.PrintQueue" /> value that represents the print queue that owns the print job.</returns>
	public PrintQueue HostingPrintQueue
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets the print server that is hosting the print queue for the print job.</summary>
	/// <returns>A <see cref="T:System.Printing.PrintServer" /> value that represents the hosting print server (usually a computer) for the <see cref="T:System.Printing.PrintQueue" /> that owns the print job.</returns>
	public PrintServer HostingPrintServer
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the print job is blocked and therefore, not printing.</summary>
	/// <returns>
	///   <see langword="true" /> if the print job is blocked; otherwise, <see langword="false" />.</returns>
	public bool IsBlocked
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the print job is finished.</summary>
	/// <returns>
	///   <see langword="true" /> if the print job is finished; otherwise, <see langword="false" />.</returns>
	public bool IsCompleted
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the print job, which is represented by the <see cref="T:System.Printing.PrintSystemJobInfo" /> object, was deleted from the print queue.</summary>
	/// <returns>
	///   <see langword="true" /> if the print job was deleted; otherwise <see langword="false" />.</returns>
	public bool IsDeleted
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the print job is being deleted from the print queue.</summary>
	/// <returns>
	///   <see langword="true" /> if the print job is being deleted; otherwise <see langword="false" />.</returns>
	public bool IsDeleting
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether an error condition is associated with the print job.</summary>
	/// <returns>
	///   <see langword="true" /> if an error condition is associated with the print job; otherwise <see langword="false" />.</returns>
	public bool IsInError
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the printer is offline.</summary>
	/// <returns>
	///   <see langword="true" /> if the printer is offline; otherwise <see langword="false" />.</returns>
	public bool IsOffline
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the printer has run out of the paper size and type that the print job requires.</summary>
	/// <returns>
	///   <see langword="true" /> if the printer has run out of the required paper; otherwise, <see langword="false" />.</returns>
	public bool IsPaperOut
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the print job is paused.</summary>
	/// <returns>
	///   <see langword="true" /> if the print job is paused; otherwise <see langword="false" />.</returns>
	public bool IsPaused
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the print job printed.</summary>
	/// <returns>
	///   <see langword="true" /> if the print job has printed; otherwise <see langword="false" />.</returns>
	public bool IsPrinted
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the print job is being printed.</summary>
	/// <returns>
	///   <see langword="true" /> if the printer is printing; otherwise <see langword="false" />.</returns>
	public bool IsPrinting
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the print job has been restarted.</summary>
	/// <returns>
	///   <see langword="true" /> if the printer is printing; otherwise <see langword="false" />.</returns>
	public bool IsRestarted
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the print job was saved in the queue after it printed.</summary>
	/// <returns>
	///   <see langword="true" /> if the print job was saved; otherwise <see langword="false" />.</returns>
	public bool IsRetained
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the print job is being spooled.</summary>
	/// <returns>
	///   <see langword="true" /> if the print job is being spooled; otherwise <see langword="false" />.</returns>
	public bool IsSpooling
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the printer needs user intervention.</summary>
	/// <returns>
	///   <see langword="true" /> if user intervention is needed; otherwise <see langword="false" />.</returns>
	public bool IsUserInterventionRequired
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets the identification number for the print job.</summary>
	/// <returns>An <see cref="T:System.Int32" /> that identifies the print job.</returns>
	public int JobIdentifier
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets or sets a name for the print job.</summary>
	/// <returns>A <see cref="T:System.String" /> name for the print job.</returns>
	public string JobName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Get the size, in bytes, of the print job.</summary>
	/// <returns>An <see cref="T:System.Int32" /> that states the size, in bytes, of the print job.</returns>
	public int JobSize
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets the current status of the print job.</summary>
	/// <returns>A <see cref="T:System.Printing.PrintJobStatus" /> value.</returns>
	public PrintJobStatus JobStatus
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a reference to the <see cref="T:System.IO.Stream" /> of the print job.</summary>
	/// <returns>A <see cref="T:System.IO.Stream" /> that contains the print job.</returns>
	public Stream JobStream
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets the number of pages in the print job.</summary>
	/// <returns>An <see cref="T:System.Int32" /> that states the number of pages in the print job.</returns>
	public int NumberOfPages
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets the number of pages that have already printed.</summary>
	/// <returns>An <see cref="T:System.Int32" /> that states the number of pages that have already printed.</returns>
	public int NumberOfPagesPrinted
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets the print job's place in the print queue.</summary>
	/// <returns>An <see cref="T:System.Int32" /> that states the print job's place in the queue.</returns>
	public int PositionInPrintQueue
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a non-numerical expression that represents the priority of the print job relative to other jobs in the print queue.</summary>
	/// <returns>A <see cref="T:System.Printing.PrintJobPriority" /> that represents the priority of the print job as <see cref="F:System.Printing.PrintJobPriority.Maximum" />, <see cref="F:System.Printing.PrintJobPriority.Minimum" />, <see cref="F:System.Printing.PrintJobPriority.Default" />, or <see cref="F:System.Printing.PrintJobPriority.None" />.</returns>
	public PrintJobPriority Priority
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets the earliest time of day, expressed as the number of minutes after midnight Coordinated Universal Time (UTC) (also called Greenwich Mean Time [GMT]), that the print job can begin printing.</summary>
	/// <returns>An <see cref="T:System.Int32" /> specifying the earliest possible start time for the print job, expressed as the number of minutes after midnight (UTC). The maximum value is 1439.</returns>
	public int StartTimeOfDay
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets the name of the user who submitted the print job.</summary>
	/// <returns>A <see cref="T:System.String" /> that identifies the user who submitted the print job.</returns>
	public string Submitter
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets the date and time that the print job is submitted.</summary>
	/// <returns>A <see cref="T:System.DateTime" /> object containing the date and time that the print job is submitted.</returns>
	public DateTime TimeJobSubmitted
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets the time, in milliseconds, since the print job started printing.</summary>
	/// <returns>An <see cref="T:System.Int32" /> that represents the time, in milliseconds, since the print job started.</returns>
	public int TimeSinceStartedPrinting
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets the last time of day, expressed as the number of minutes after midnight Coordinated Universal Time (UTC) (also called Greenwich Mean Time [GMT]), that the print job can begin printing.</summary>
	/// <returns>An <see cref="T:System.Int32" /> that specifies the last time that the job can print, expressed as the number of minutes after midnight (UTC). The maximum value is 1439.</returns>
	public int UntilTimeOfDay
	{
		get
		{
			throw null;
		}
	}

	internal PrintSystemJobInfo()
	{
	}

	/// <summary>Cancels the print job.</summary>
	public void Cancel()
	{
	}

	/// <summary>Writes any changes to the properties of the <see cref="T:System.Printing.PrintSystemJobInfo" /> object to the actual print job that the object represents.</summary>
	public override void Commit()
	{
	}

	/// <summary>Gets the <see cref="T:System.Printing.PrintSystemJobInfo" /> for the specified job in the specified <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="printQueue">The print queue hosting the print job.</param>
	/// <param name="jobIdentifier">A numerical ID for the print job.</param>
	/// <returns>The <see cref="T:System.Printing.PrintSystemJobInfo" /> that corresponds to the <paramref name="jobIdentifier" />.</returns>
	public static PrintSystemJobInfo Get(PrintQueue printQueue, int jobIdentifier)
	{
		throw null;
	}

	/// <summary>Releases the unmanaged resources that are used by the <see cref="T:System.Printing.PrintSystemJobInfo" /> and optionally, releases the managed resources.</summary>
	/// <param name="disposing">
	///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
	protected sealed override void InternalDispose(bool disposing)
	{
	}

	/// <summary>Halts printing of the job until <see cref="M:System.Printing.PrintSystemJobInfo.Resume" /> runs.</summary>
	public void Pause()
	{
	}

	/// <summary>Updates the properties of the <see cref="T:System.Printing.PrintSystemJobInfo" /> object so that their values match the values of the actual print job that the object represents.</summary>
	public override void Refresh()
	{
	}

	/// <summary>Restarts a print job from the beginning.</summary>
	public void Restart()
	{
	}

	/// <summary>Resumes the printing of a paused print job.</summary>
	public void Resume()
	{
	}
}
