namespace System.Printing;

/// <summary>Specifies the intended meaning of a <see cref="T:System.Printing.PrintQueueStringProperty" />.</summary>
public enum PrintQueueStringPropertyType
{
	/// <summary>A comment about the print queue.</summary>
	Comment = 1,
	/// <summary>The location of the physical printer.</summary>
	Location = 0,
	/// <summary>The share name of the print queue.</summary>
	ShareName = 2
}
