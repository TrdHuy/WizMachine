namespace System.Printing.IndexedProperties;

/// <summary>Represents a <see cref="T:System.Printing.PrintQueueStatus" /> property (and its value) of a printing system hardware or software component.</summary>
public sealed class PrintQueueStatusProperty : PrintProperty
{
	/// <summary>Gets or sets the value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintQueueStatusProperty" /> represents.</summary>
	/// <exception cref="T:System.InvalidOperationException">The property cannot be set by using the value that is provided by the calling code.</exception>
	/// <returns>A boxed <see cref="T:System.Printing.PrintQueueStatus" />.</returns>
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

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintQueueStatusProperty" /> class for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Printing.PrintQueueStatus" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintQueueStatusProperty" /> represents.</param>
	public PrintQueueStatusProperty(string attributeName)
		: base(null)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintQueueStatusProperty" /> class that has the specified value for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Printing.PrintQueueStatus" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintQueueStatusProperty" /> represents.</param>
	/// <param name="attributeValue">The value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintQueueStatusProperty" /> represents.</param>
	public PrintQueueStatusProperty(string attributeName, object attributeValue)
		: base(null)
	{
	}

	protected sealed override void InternalDispose(bool disposing)
	{
	}

	public static implicit operator PrintQueueStatus(PrintQueueStatusProperty attributeRef)
	{
		throw null;
	}
}
