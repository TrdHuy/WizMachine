namespace System.Printing.IndexedProperties;

/// <summary>Represents a property of a printing system hardware or software component whose value is an array of <see cref="T:System.Byte" /> values.</summary>
public sealed class PrintByteArrayProperty : PrintProperty
{
	/// <summary>Gets or sets the value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintByteArrayProperty" /> represents.</summary>
	/// <returns>A boxed array of <see cref="T:System.Byte" /> values.</returns>
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

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintByteArrayProperty" /> class for the specified attribute.</summary>
	/// <param name="attributeName">The name of the property, which is an array of type <see cref="T:System.Byte" />, that the <see cref="T:System.Printing.IndexedProperties.PrintByteArrayProperty" /> represents.</param>
	public PrintByteArrayProperty(string attributeName)
		: base(null)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintByteArrayProperty" /> class that has the specified value for the specified attribute.</summary>
	/// <param name="attributeName">The name of the property, which is an array of type <see cref="T:System.Byte" />, that the <see cref="T:System.Printing.IndexedProperties.PrintByteArrayProperty" /> represents.</param>
	/// <param name="attributeValue">The value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintByteArrayProperty" /> represents.</param>
	public PrintByteArrayProperty(string attributeName, object attributeValue)
		: base(null)
	{
	}

	protected sealed override void InternalDispose(bool disposing)
	{
	}

	/// <summary>Provides implicit conversion to an array of <see cref="T:System.Byte" /> values from a pointer to a <see cref="T:System.Printing.IndexedProperties.PrintByteArrayProperty" />.</summary>
	/// <param name="attribRef">A pointer to a <see cref="T:System.Printing.IndexedProperties.PrintByteArrayProperty" /> that is converted.</param>
	/// <returns>An array of <see cref="T:System.Byte" /> values.</returns>
	public static implicit operator byte[](PrintByteArrayProperty attribRef)
	{
		throw null;
	}
}
