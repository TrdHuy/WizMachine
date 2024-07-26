namespace System.Printing;

/// <summary>Specifies whether the print job uses XML Paper Specification (XPS).</summary>
public enum PrintJobType
{
	/// <summary>A non-XPS print job.</summary>
	Legacy = 2,
	/// <summary>Not specified whether the print job is XPS.</summary>
	None = 0,
	/// <summary>An XPS print job.</summary>
	Xps = 1
}
