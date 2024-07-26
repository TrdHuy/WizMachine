namespace System.Printing.IndexedProperties;

/// <summary>Represents a <see cref="T:System.Printing.PrintQueue" /> property (and its value) of a printing system hardware or software component.</summary>
public sealed class PrintQueueProperty : PrintProperty
{
	/// <summary>Gets or sets the value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintQueueProperty" /> represents.</summary>
	/// <returns>An <see cref="T:System.Object" /> that can be cast to a <see cref="T:System.Printing.PrintQueue" />.</returns>
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

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintQueueProperty" /> class for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Printing.PrintQueue" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintQueueProperty" /> represents.</param>
	public PrintQueueProperty(string attributeName)
		: base(null)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintQueueProperty" /> class that has the specified value for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Printing.PrintQueue" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintQueueProperty" /> represents.</param>
	/// <param name="attributeValue">The value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintQueueProperty" /> represents.</param>
	public PrintQueueProperty(string attributeName, object attributeValue)
		: base(null)
	{
	}

	protected sealed override void InternalDispose(bool disposing)
	{
	}

	/// <summary>Provides implicit conversion to a <see cref="T:System.Printing.PrintQueue" /> value from a pointer to a <see cref="T:System.Printing.IndexedProperties.PrintQueueProperty" />.</summary>
	/// <param name="attribRef">A pointer to the <see cref="T:System.Printing.IndexedProperties.PrintQueueProperty" /> that is converted.</param>
	/// <returns>A <see cref="T:System.Printing.PrintQueue" /> that is the converted value of the <see cref="T:System.Printing.IndexedProperties.PrintQueueProperty" />.</returns>
	public static implicit operator PrintQueue(PrintQueueProperty attribRef)
	{
		throw null;
	}
}
