namespace System.Printing.IndexedProperties;

/// <summary>Represents a <see cref="T:System.Printing.PrintServer" /> property (and its value) of a printing system hardware or software component.</summary>
public sealed class PrintServerProperty : PrintProperty
{
	/// <summary>Gets or sets the value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintServerProperty" /> represents.</summary>
	/// <exception cref="T:System.InvalidOperationException">The property is not internally initialized.</exception>
	/// <returns>A <see cref="T:System.Object" /> that can be cast as a <see cref="T:System.Printing.PrintServer" />.</returns>
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

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintServerProperty" /> class for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Printing.PrintServer" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintServerProperty" /> represents.</param>
	public PrintServerProperty(string attributeName)
		: base(null)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintServerProperty" /> class that has the specified value for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Printing.PrintServer" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintServerProperty" /> represents.</param>
	/// <param name="attributeValue">The value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintServerProperty" /> represents.</param>
	public PrintServerProperty(string attributeName, object attributeValue)
		: base(null)
	{
	}

	protected sealed override void InternalDispose(bool disposing)
	{
	}

	/// <summary>Provides implicit conversion to a <see cref="T:System.Printing.PrintServer" /> value from a pointer to a <see cref="T:System.Printing.IndexedProperties.PrintServerProperty" />.</summary>
	/// <param name="attribRef">A pointer to the <see cref="T:System.Printing.IndexedProperties.PrintServerProperty" /> that is converted.</param>
	/// <returns>The <see cref="T:System.Printing.PrintServer" /> that is the converted value.</returns>
	public static implicit operator PrintServer(PrintServerProperty attribRef)
	{
		throw null;
	}
}
