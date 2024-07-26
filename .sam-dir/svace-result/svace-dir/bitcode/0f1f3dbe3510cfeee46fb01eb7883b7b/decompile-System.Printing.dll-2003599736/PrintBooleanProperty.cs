namespace System.Printing.IndexedProperties;

/// <summary>Represents a <see cref="T:System.Boolean" /> property (and its value) of a printing system hardware or software component.</summary>
public sealed class PrintBooleanProperty : PrintProperty
{
	/// <summary>Gets or sets the value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintBooleanProperty" /> represents.</summary>
	/// <returns>A boxed <see cref="T:System.Boolean" />.</returns>
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

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintBooleanProperty" /> class for the specified attribute.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Boolean" /> attribute that the <see cref="T:System.Printing.IndexedProperties.PrintBooleanProperty" /> represents.</param>
	public PrintBooleanProperty(string attributeName)
		: base(null)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintBooleanProperty" /> class for the specified property that is using the specified value.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Boolean" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintBooleanProperty" /> represents.</param>
	/// <param name="attributeValue">The value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintBooleanProperty" /> represents.</param>
	public PrintBooleanProperty(string attributeName, object attributeValue)
		: base(null)
	{
	}

	protected sealed override void InternalDispose(bool disposing)
	{
	}

	/// <summary>Provides implicit conversion to a <see cref="T:System.Boolean" /> from a pointer to a <see cref="T:System.Printing.IndexedProperties.PrintBooleanProperty" />.</summary>
	/// <param name="attribRef">A pointer to the <see cref="T:System.Printing.IndexedProperties.PrintBooleanProperty" /> that is converted.</param>
	/// <returns>The converted <see cref="T:System.Printing.IndexedProperties.PrintBooleanProperty" /> object as a Boolean object.</returns>
	public static implicit operator bool(PrintBooleanProperty attribRef)
	{
		throw null;
	}
}
