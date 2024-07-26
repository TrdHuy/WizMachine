namespace System.Printing;

/// <summary>Specifies the different access rights (or levels of access) for printing objects.</summary>
public enum PrintSystemDesiredAccess
{
	/// <summary>The right to perform all administrative tasks for the print queue, including the right to pause and resume any print job; and the right to delete all jobs from the queue. This access level also includes all rights under <see cref="F:System.Printing.PrintSystemDesiredAccess.UsePrinter" />.</summary>
	AdministratePrinter = 983052,
	/// <summary>The right to perform all administrative tasks for the print server. This access level does not include <see cref="F:System.Printing.PrintSystemDesiredAccess.AdministratePrinter" /> rights for the print queues hosted by the server.</summary>
	AdministrateServer = 983041,
	/// <summary>The right to list the queues on the print server.</summary>
	EnumerateServer = 131074,
	/// <summary>No access.</summary>
	None = 0,
	/// <summary>The right to add print jobs to the queue and to delete and enumerate one's own jobs.</summary>
	UsePrinter = 131080
}
