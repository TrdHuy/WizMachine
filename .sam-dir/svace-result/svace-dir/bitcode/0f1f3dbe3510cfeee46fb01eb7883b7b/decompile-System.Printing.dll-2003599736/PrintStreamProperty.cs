using System.IO;

namespace System.Printing.IndexedProperties;

/// <summary>Represents a <see cref="T:System.IO.Stream" /> property (and its value) of a printing system hardware or software component.</summary>
public sealed class PrintStreamProperty : PrintProperty
{
	/// <summary>Gets or sets the value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintStreamProperty" /> represents.</summary>
	/// <returns>An <see cref="T:System.Object" /> that can be cast as a <see cref="T:System.IO.Stream" />.</returns>
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

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintStreamProperty" /> class for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.IO.Stream" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintStreamProperty" /> represents.</param>
	public PrintStreamProperty(string attributeName)
		: base(null)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintStreamProperty" /> class that has the specified value for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.IO.Stream" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintStreamProperty" /> represents.</param>
	/// <param name="attributeValue">The value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintStreamProperty" /> represents.</param>
	public PrintStreamProperty(string attributeName, object attributeValue)
		: base(null)
	{
	}

	protected sealed override void InternalDispose(bool disposing)
	{
	}

	/// <summary>Provides implicit conversion to a <see cref="T:System.IO.Stream" /> value from a pointer to a <see cref="T:System.Printing.IndexedProperties.PrintStreamProperty" />.</summary>
	/// <param name="attributeRef">A pointer to the <see cref="T:System.Printing.IndexedProperties.PrintStreamProperty" /> that is converted.</param>
	/// <returns>A <see cref="T:System.IO.Stream" /> value of a <see cref="T:System.Printing.IndexedProperties.PrintStreamProperty" />.</returns>
	public static implicit operator Stream(PrintStreamProperty attributeRef)
	{
		throw null;
	}
}
