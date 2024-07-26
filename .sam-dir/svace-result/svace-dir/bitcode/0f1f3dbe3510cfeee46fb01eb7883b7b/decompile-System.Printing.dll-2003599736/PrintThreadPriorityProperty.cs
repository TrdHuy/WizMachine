using System.Threading;

namespace System.Printing.IndexedProperties;

/// <summary>Represents a <see cref="T:System.Threading.ThreadPriority" /> property (and its value) of a printing system hardware or software component.</summary>
public sealed class PrintThreadPriorityProperty : PrintProperty
{
	/// <summary>Gets or sets the value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintThreadPriorityProperty" /> represents.</summary>
	/// <returns>A boxed <see cref="T:System.Threading.ThreadPriority" /> value.</returns>
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

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintThreadPriorityProperty" /> class for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Threading.ThreadPriority" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintThreadPriorityProperty" /> represents.</param>
	public PrintThreadPriorityProperty(string attributeName)
		: base(null)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintThreadPriorityProperty" /> class that has the specified value for the specified property.</summary>
	/// <param name="attributeName">The name of the <see cref="T:System.Threading.ThreadPriority" /> property that the <see cref="T:System.Printing.IndexedProperties.PrintThreadPriorityProperty" /> represents.</param>
	/// <param name="attributeValue">The value of the property that the <see cref="T:System.Printing.IndexedProperties.PrintThreadPriorityProperty" /> represents.</param>
	public PrintThreadPriorityProperty(string attributeName, object attributeValue)
		: base(null)
	{
	}

	protected sealed override void InternalDispose(bool disposing)
	{
	}

	/// <summary>Provides implicit conversion to a <see cref="T:System.Threading.ThreadPriority" /> value from a pointer to a <see cref="T:System.Printing.IndexedProperties.PrintThreadPriorityProperty" />.</summary>
	/// <param name="attribRef">A pointer to the <see cref="T:System.Printing.IndexedProperties.PrintThreadPriorityProperty" /> that is converted.</param>
	/// <returns>A <see cref="T:System.Threading.ThreadPriority" /> value.</returns>
	public static implicit operator ThreadPriority(PrintThreadPriorityProperty attribRef)
	{
		throw null;
	}
}
