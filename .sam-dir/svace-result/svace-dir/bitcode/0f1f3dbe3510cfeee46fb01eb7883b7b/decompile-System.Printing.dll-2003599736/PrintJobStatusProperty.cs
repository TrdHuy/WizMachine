namespace System.Printing.IndexedProperties;

/// <summary>Represents a <see cref="T:System.Printing.PrintJobStatus" /> property (and its value) of a printing system hardware or software component.</summary>
public sealed class PrintJobStatusProperty : PrintProperty
{
	/// <summary>Gets or sets the value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintJobStatusProperty" /> represents.</summary>
	/// <returns>A boxed <see cref="T:System.Printing.PrintJobStatus" /> value.</returns>
	public override object Value
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintJobStatusProperty" /> class for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Printing.PrintJobStatus" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintJobStatusProperty" /> represents.</param>
	public PrintJobStatusProperty(string attributeName)
		: base(null)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintJobStatusProperty" /> class that has the specified value for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Printing.PrintJobStatus" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintJobStatusProperty" /> represents.</param>
	/// <param name="attributeValue">The value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintJobStatusProperty" /> represents.</param>
	public PrintJobStatusProperty(string attributeName, object attributeValue)
		: base(null)
	{
	}

	protected sealed override void InternalDispose(bool disposing)
	{
	}

	/// <summary>Provides implicit conversion to a <see cref="T:System.Printing.PrintJobStatus" /> value from a pointer to a <see cref="T:System.Printing.IndexedProperties.PrintJobStatusProperty" />.</summary>
	/// <param name="attribRef">A pointer to the <see cref="T:System.Printing.IndexedProperties.PrintJobStatusProperty" /> that is converted.</param>
	/// <returns>A <see cref="T:System.Printing.PrintJobStatus" /> that is the converted value of the pointer.</returns>
	public static implicit operator PrintJobStatus(PrintJobStatusProperty attribRef)
	{
		throw null;
	}
}
