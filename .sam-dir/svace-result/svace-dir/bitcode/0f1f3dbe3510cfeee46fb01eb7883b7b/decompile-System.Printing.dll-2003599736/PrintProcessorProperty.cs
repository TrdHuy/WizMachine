namespace System.Printing.IndexedProperties;

/// <summary>Represents a <see cref="T:System.Printing.PrintProcessor" /> property (and its value) of a printing system hardware or software component.</summary>
public sealed class PrintProcessorProperty : PrintProperty
{
	/// <summary>Gets or sets the value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintProcessorProperty" /> represents.</summary>
	/// <returns>An <see cref="T:System.Object" /> that can be cast to a <see cref="T:System.Printing.PrintProcessor" />.</returns>
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

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintProcessorProperty" /> class for the specified property.</summary>
	/// <param name="attributeName">The <see cref="T:System.Printing.IndexedProperties.PrintProcessorProperty" /> that is converted.</param>
	public PrintProcessorProperty(string attributeName)
		: base(null)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintProcessorProperty" /> class that has the specified value for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Printing.PrintProcessor" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintProcessorProperty" /> represents.</param>
	/// <param name="attributeValue">The value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintProcessorProperty" /> represents.</param>
	public PrintProcessorProperty(string attributeName, object attributeValue)
		: base(null)
	{
	}

	protected sealed override void InternalDispose(bool disposing)
	{
	}

	/// <summary>Provides implicit conversion to a <see cref="T:System.Printing.PrintProcessor" /> value from a pointer to a <see cref="T:System.Printing.IndexedProperties.PrintProcessorProperty" />.</summary>
	/// <param name="attribRef">A pointer to the <see cref="T:System.Printing.IndexedProperties.PrintProcessorProperty" /> that is converted.</param>
	/// <returns>A <see cref="T:System.Printing.PrintProcessor" /> value of the <see cref="T:System.Printing.IndexedProperties.PrintProcessorProperty" />.</returns>
	public static implicit operator PrintProcessor(PrintProcessorProperty attribRef)
	{
		throw null;
	}
}
