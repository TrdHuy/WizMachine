namespace System.Printing.IndexedProperties;

/// <summary>Represents a <see cref="T:System.Printing.PrintJobPriority" /> property (and its value) of a printing system hardware or software component.</summary>
public sealed class PrintJobPriorityProperty : PrintProperty
{
	/// <summary>Gets or sets the value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintJobPriorityProperty" /> represents.</summary>
	/// <returns>A boxed <see cref="T:System.Printing.PrintJobPriority" /> value.</returns>
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

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintJobPriorityProperty" /> class for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Printing.PrintJobPriority" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintJobPriorityProperty" /> represents.</param>
	public PrintJobPriorityProperty(string attributeName)
		: base(null)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintJobPriorityProperty" /> class that has the specified value for the specified attribute.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Printing.PrintJobPriority" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintJobPriorityProperty" /> represents.</param>
	/// <param name="attributeValue">The value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintJobPriorityProperty" /> represents.</param>
	public PrintJobPriorityProperty(string attributeName, object attributeValue)
		: base(null)
	{
	}

	protected sealed override void InternalDispose(bool disposing)
	{
	}

	/// <summary>Provides implicit conversion to a <see cref="T:System.Printing.PrintJobPriority" /> from a pointer to a <see cref="T:System.Printing.IndexedProperties.PrintJobPriorityProperty" />.</summary>
	/// <param name="attribRef">A pointer to the <see cref="T:System.Printing.IndexedProperties.PrintBooleanProperty" /> that is converted.</param>
	/// <returns>A <see cref="T:System.Printing.PrintJobPriority" /> value.</returns>
	public static implicit operator PrintJobPriority(PrintJobPriorityProperty attribRef)
	{
		throw null;
	}
}
