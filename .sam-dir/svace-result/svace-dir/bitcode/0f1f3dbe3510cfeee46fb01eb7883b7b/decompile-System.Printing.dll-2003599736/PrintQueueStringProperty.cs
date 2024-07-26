namespace System.Printing;

/// <summary>Represents one, and only one, of three possible properties of a print queue: <see cref="P:System.Printing.PrintQueue.Location" />, <see cref="P:System.Printing.PrintQueue.Comment" />, or <see cref="P:System.Printing.PrintQueue.ShareName" />.</summary>
public class PrintQueueStringProperty
{
	/// <summary>Gets or sets the value of the print queue property that is represented.</summary>
	/// <returns>A <see cref="T:System.String" /> with the value of the property.</returns>
	public string Name
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets or sets a value that identifies which of the three possible properties of a print queue is being represented.</summary>
	/// <returns>A <see cref="T:System.Printing.PrintQueueStringPropertyType" /> that specifies the kind of print queue property that is being represented.</returns>
	public PrintQueueStringPropertyType Type
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.PrintQueueStringProperty" /> class.</summary>
	public PrintQueueStringProperty()
	{
	}
}
