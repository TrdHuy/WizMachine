namespace System.Printing.IndexedProperties;

/// <summary>Represents a <see cref="T:System.Type" /> property (and its value) of a printing system hardware or software component.</summary>
public sealed class PrintSystemTypeProperty : PrintProperty
{
	/// <summary>Gets or sets the value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintSystemTypeProperty" /> represents.</summary>
	/// <returns>The value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintSystemTypeProperty" /> represents.</returns>
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

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintSystemTypeProperty" /> class that has the specified property name.</summary>
	/// <param name="attributeName">The name of the property that the <see cref="T:System.Printing.IndexedProperties.PrintSystemTypeProperty" /> represents.</param>
	public PrintSystemTypeProperty(string attributeName)
		: base(null)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintSystemTypeProperty" /> class that has the specified property name and value.</summary>
	/// <param name="attributeName">The name of the property that the <see cref="T:System.Printing.IndexedProperties.PrintSystemTypeProperty" /> represents.</param>
	/// <param name="attributeValue">The value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintSystemTypeProperty" /> represents.</param>
	public PrintSystemTypeProperty(string attributeName, object attributeValue)
		: base(null)
	{
	}

	protected sealed override void InternalDispose(bool disposing)
	{
	}

	/// <summary>Provides implicit conversion to a <see cref="T:System.Type" /> value from a pointer to a <see cref="T:System.Printing.IndexedProperties.PrintSystemTypeProperty" />.</summary>
	/// <param name="attribRef">A pointer to the <see cref="T:System.Printing.IndexedProperties.PrintSystemTypeProperty" /> that is converted.</param>
	/// <returns>A converted Type value form a  PrintSystemTypeProperty object.</returns>
	public static implicit operator Type(PrintSystemTypeProperty attribRef)
	{
		throw null;
	}
}
