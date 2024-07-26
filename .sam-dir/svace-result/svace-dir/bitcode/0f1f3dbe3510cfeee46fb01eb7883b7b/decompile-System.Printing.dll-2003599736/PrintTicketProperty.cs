namespace System.Printing.IndexedProperties;

/// <summary>Represents a <see cref="T:System.Printing.PrintTicket" /> property (and its value) of a printing system hardware or software component.</summary>
public sealed class PrintTicketProperty : PrintProperty
{
	/// <summary>Gets or sets the value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintTicketProperty" /> represents.</summary>
	/// <returns>An <see cref="T:System.Object" /> that can be cast to a <see cref="T:System.Printing.PrintTicket" />.</returns>
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

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintTicketProperty" /> class for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Printing.PrintTicket" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintTicketProperty" /> represents.</param>
	public PrintTicketProperty(string attributeName)
		: base(null)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintTicketProperty" /> class that has the specified value for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Printing.PrintTicket" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintTicketProperty" /> represents.</param>
	/// <param name="attributeValue">The value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintTicketProperty" /> represents.</param>
	public PrintTicketProperty(string attributeName, object attributeValue)
		: base(null)
	{
	}

	protected sealed override void InternalDispose(bool disposing)
	{
	}

	/// <summary>Provides implicit conversion to a <see cref="T:System.Printing.PrintTicket" /> value from a pointer to a <see cref="T:System.Printing.IndexedProperties.PrintTicketProperty" />.</summary>
	/// <param name="attribRef">A pointer to the <see cref="T:System.Printing.IndexedProperties.PrintTicketProperty" /> that is converted.</param>
	/// <returns>A converted PrintTicketProperty object to a PrintTicket value.</returns>
	public static implicit operator PrintTicket(PrintTicketProperty attribRef)
	{
		throw null;
	}
}
