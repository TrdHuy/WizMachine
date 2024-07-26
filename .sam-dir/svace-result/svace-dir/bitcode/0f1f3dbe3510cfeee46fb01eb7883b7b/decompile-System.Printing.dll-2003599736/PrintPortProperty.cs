namespace System.Printing.IndexedProperties;

/// <summary>Represents a <see cref="T:System.Printing.PrintPort" /> property (and its value) of a printing system hardware or software component.</summary>
public sealed class PrintPortProperty : PrintProperty
{
	/// <summary>Gets or sets the value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintPortProperty" /> represents.</summary>
	/// <returns>An <see cref="T:System.Object" /> that can be cast to a <see cref="T:System.Printing.IndexedProperties.PrintPortProperty" />.</returns>
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

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintPortProperty" /> class for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Printing.PrintPort" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintPortProperty" /> represents.</param>
	public PrintPortProperty(string attributeName)
		: base(null)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintPortProperty" /> class that has the specified value for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Printing.PrintPort" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintPortProperty" /> represents.</param>
	/// <param name="attributeValue">The value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintPortProperty" /> represents.</param>
	public PrintPortProperty(string attributeName, object attributeValue)
		: base(null)
	{
	}

	protected sealed override void InternalDispose(bool disposing)
	{
	}

	/// <summary>Provides implicit conversion to a <see cref="T:System.Printing.PrintPort" /> value from a pointer to a <see cref="T:System.Printing.IndexedProperties.PrintPortProperty" />.</summary>
	/// <param name="attribRef">A pointer to the <see cref="T:System.Printing.IndexedProperties.PrintPortProperty" /> that is converted.</param>
	/// <returns>The converted PrintPort value form a PrintPortProperty object.</returns>
	public static implicit operator PrintPort(PrintPortProperty attribRef)
	{
		throw null;
	}
}
