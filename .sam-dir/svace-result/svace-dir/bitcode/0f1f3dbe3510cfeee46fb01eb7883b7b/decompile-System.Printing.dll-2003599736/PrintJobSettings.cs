namespace System.Printing;

/// <summary>Describes a print job.</summary>
public class PrintJobSettings
{
	/// <summary>Gets or sets a <see cref="T:System.Printing.PrintTicket" /> object that holds all the detailed settings for the print job.</summary>
	/// <returns>A <see cref="T:System.Printing.PrintTicket" /> object that holds all the details about the print job, such as the number of copies to print, and whether stapling or duplex printing is used.</returns>
	public PrintTicket CurrentPrintTicket
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets or sets a description of a print job.</summary>
	/// <returns>A <see cref="T:System.String" /> that describes the print job, for example, "Quarterly Report."</returns>
	public string Description
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	internal PrintJobSettings()
	{
	}
}
