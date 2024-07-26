namespace System.Printing.IndexedProperties;

/// <summary>Represents a <see cref="T:System.Printing.PrintDriver" /> property (and its value) of a printing system hardware or software component.</summary>
public sealed class PrintDriverProperty : PrintProperty
{
	/// <summary>Gets or sets the value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintDriverProperty" /> represents.</summary>
	/// <returns>An <see cref="T:System.Object" /> that can be cast to a <see cref="T:System.Printing.PrintDriver" />.</returns>
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

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintDriverProperty" /> class for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Printing.PrintDriver" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintDriverProperty" /> represents.</param>
	public PrintDriverProperty(string attributeName)
		: base(null)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintDriverProperty" /> class that has the specified value for the specified attribute.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Printing.PrintDriver" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintDriverProperty" /> represents.</param>
	/// <param name="attributeValue">The value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintDriverProperty" /> represents.</param>
	public PrintDriverProperty(string attributeName, object attributeValue)
		: base(null)
	{
	}

	protected sealed override void InternalDispose(bool disposing)
	{
	}

	/// <summary>Provides implicit conversion to a <see cref="T:System.Printing.PrintDriver" /> from a pointer to a <see cref="T:System.Printing.IndexedProperties.PrintDriverProperty" />.</summary>
	/// <param name="attribRef">The pointer to a <see cref="T:System.Printing.IndexedProperties.PrintDriverProperty" /> that is converted.</param>
	/// <returns>A <see cref="T:System.Printing.PrintDriver" /> that is the converted value of the <see cref="T:System.Printing.IndexedProperties.PrintDriverProperty" />.</returns>
	public static implicit operator PrintDriver(PrintDriverProperty attribRef)
	{
		throw null;
	}
}
