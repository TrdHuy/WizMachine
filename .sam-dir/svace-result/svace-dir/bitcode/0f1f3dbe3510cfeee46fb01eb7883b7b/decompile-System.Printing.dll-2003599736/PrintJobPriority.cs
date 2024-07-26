namespace System.Printing;

/// <summary>Specifies a non-numerical priority for a print job relative to other print jobs in the print queue.</summary>
public enum PrintJobPriority
{
	/// <summary>A job that has the <see cref="P:System.Printing.PrintQueue.DefaultPriority" /> for the <see cref="T:System.Printing.PrintQueueStream" />.</summary>
	Default = 1,
	/// <summary>A job that has the highest priority.</summary>
	Maximum = 99,
	/// <summary>A job that has the lowest priority.</summary>
	Minimum = 1,
	/// <summary>A job that has no non-numerical priority.</summary>
	None = 0
}
