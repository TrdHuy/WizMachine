namespace System.Printing.IndexedProperties;

/// <summary>Represents a <see cref="T:System.String" /> property (and its value) of a printing system hardware or software component.</summary>
public sealed class PrintStringProperty : PrintProperty
{
	/// <summary>Gets or sets the value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintStringProperty" /> represents.</summary>
	/// <returns>A boxed <see cref="T:System.String" />.</returns>
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

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintStringProperty" /> class for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.String" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintStringProperty" /> represents.</param>
	public PrintStringProperty(string attributeName)
		: base(null)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintStringProperty" /> class that has the specified value for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.String" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintStringProperty" /> represents.</param>
	/// <param name="attributeValue">The value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintStringProperty" /> represents.</param>
	public PrintStringProperty(string attributeName, object attributeValue)
		: base(null)
	{
	}

	protected sealed override void InternalDispose(bool disposing)
	{
	}

	public static implicit operator string(PrintStringProperty attributeRef)
	{
		throw null;
	}
}
