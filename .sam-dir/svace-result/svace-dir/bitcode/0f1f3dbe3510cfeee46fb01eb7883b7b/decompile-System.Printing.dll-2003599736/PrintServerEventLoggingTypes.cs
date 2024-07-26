namespace System.Printing;

/// <summary>Specifies the types of events that can be logged by a <see cref="T:System.Printing.PrintServer" />.</summary>
[Flags]
public enum PrintServerEventLoggingTypes
{
	/// <summary>All printing events.</summary>
	LogAllPrintingEvents = 5,
	/// <summary>Error events for printing.</summary>
	LogPrintingErrorEvents = 2,
	/// <summary>Information events for printing.</summary>
	LogPrintingInformationEvents = 4,
	/// <summary>Successful printing events.</summary>
	LogPrintingSuccessEvents = 1,
	/// <summary>Warning events.</summary>
	LogPrintingWarningEvents = 3,
	/// <summary>No events.</summary>
	None = 0
}
