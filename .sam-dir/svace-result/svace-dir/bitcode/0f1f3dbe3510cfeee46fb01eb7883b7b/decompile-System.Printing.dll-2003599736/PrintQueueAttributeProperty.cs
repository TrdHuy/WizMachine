namespace System.Printing.IndexedProperties;

/// <summary>Represents a <see cref="T:System.Printing.PrintQueueAttributes" /> property (and its value) of a printing system hardware or software component.</summary>
public sealed class PrintQueueAttributeProperty : PrintProperty
{
	/// <summary>Gets or sets the value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintQueueAttributeProperty" /> represents.</summary>
	/// <exception cref="T:System.InvalidOperationException">The property cannot be set by using the value that is provided by the calling code.</exception>
	/// <returns>A boxed <see cref="T:System.Printing.PrintQueueAttributes" /> value.</returns>
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

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintQueueAttributeProperty" /> class for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Printing.PrintQueueAttributes" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintQueueAttributeProperty" /> represents.</param>
	public PrintQueueAttributeProperty(string attributeName)
		: base(null)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintQueueAttributeProperty" /> class that has the specified value for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Printing.PrintQueueAttributes" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintQueueAttributeProperty" /> represents.</param>
	/// <param name="attributeValue">The value of <see cref="T:System.Object" /> the property that the <see cref="T:System.Printing.IndexedProperties.PrintQueueAttributeProperty" /> represents.</param>
	public PrintQueueAttributeProperty(string attributeName, object attributeValue)
		: base(null)
	{
	}

	protected sealed override void InternalDispose(bool disposing)
	{
	}

	/// <summary>Provides implicit conversion to a <see cref="T:System.Printing.PrintQueueAttributes" /> value from a pointer to a <see cref="T:System.Printing.IndexedProperties.PrintQueueAttributeProperty" />.</summary>
	/// <param name="attributeRef">A pointer to the <see cref="T:System.Printing.IndexedProperties.PrintQueueAttributeProperty" /> that is converted.</param>
	/// <returns>A <see cref="T:System.Printing.PrintQueueAttributes" /> value.</returns>
	public static implicit operator PrintQueueAttributes(PrintQueueAttributeProperty attributeRef)
	{
		throw null;
	}
}
