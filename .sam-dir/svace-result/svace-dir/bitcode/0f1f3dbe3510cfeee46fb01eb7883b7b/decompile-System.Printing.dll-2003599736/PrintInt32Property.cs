namespace System.Printing.IndexedProperties;

/// <summary>Represents an <see cref="T:System.Int32" /> property (and the value of the property) of a printing system hardware or software component.</summary>
public sealed class PrintInt32Property : PrintProperty
{
	/// <summary>Gets or sets the value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintInt32Property" /> represents.</summary>
	/// <exception cref="T:System.InvalidOperationException">The property cannot be set to the value that the calling code provides.</exception>
	/// <returns>A boxed <see cref="T:System.Int32" />.</returns>
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

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintInt32Property" /> class for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Int32" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintInt32Property" /> represents.</param>
	public PrintInt32Property(string attributeName)
		: base(null)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintInt32Property" /> class for the specified attribute and gives it the specified value.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Int32" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintInt32Property" /> represents.</param>
	/// <param name="attributeValue">The value of <see cref="T:System.Object" /> the property that the <see cref="T:System.Printing.IndexedProperties.PrintInt32Property" /> represents.</param>
	public PrintInt32Property(string attributeName, object attributeValue)
		: base(null)
	{
	}

	protected sealed override void InternalDispose(bool disposing)
	{
	}

	/// <summary>Provides implicit conversion to an <see cref="T:System.Int32" /> from a pointer to a <see cref="T:System.Printing.IndexedProperties.PrintInt32Property" />.</summary>
	/// <param name="attribRef">A pointer to the <see cref="T:System.Printing.IndexedProperties.PrintInt32Property" /> that is converted.</param>
	/// <returns>The converted PrintInt32Property object to an Int32 object.</returns>
	public static implicit operator int(PrintInt32Property attribRef)
	{
		throw null;
	}
}
