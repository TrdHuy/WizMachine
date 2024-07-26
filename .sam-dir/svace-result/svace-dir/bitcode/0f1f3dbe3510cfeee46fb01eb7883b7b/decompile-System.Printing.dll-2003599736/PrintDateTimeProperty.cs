namespace System.Printing.IndexedProperties;

/// <summary>Represents a <see cref="T:System.DateTime" /> property (and its value) of a printing system hardware or software component.</summary>
public sealed class PrintDateTimeProperty : PrintProperty
{
	/// <summary>Gets or sets the value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintDateTimeProperty" /> represents.</summary>
	/// <returns>A boxed <see cref="T:System.DateTime" />.</returns>
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

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintDateTimeProperty" /> class for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.DateTime" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintDateTimeProperty" /> represents.</param>
	public PrintDateTimeProperty(string attributeName)
		: base(null)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintDateTimeProperty" /> class that has the specified value for the specified attribute.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.DateTime" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintDateTimeProperty" /> represents.</param>
	/// <param name="attributeValue">The value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintDateTimeProperty" /> represents.</param>
	public PrintDateTimeProperty(string attributeName, object attributeValue)
		: base(null)
	{
	}

	protected sealed override void InternalDispose(bool disposing)
	{
	}

	public static implicit operator ValueType(PrintDateTimeProperty attribRef)
	{
		throw null;
	}
}
