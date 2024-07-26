using System.Collections.Specialized;

namespace System.Printing;

/// <summary>Provides data for a PropertiesChanged event, which you must create.</summary>
public class PrintSystemObjectPropertiesChangedEventArgs : EventArgs, IDisposable
{
	/// <summary>Gets a collection of the names of the changed properties.</summary>
	/// <returns>A <see cref="T:System.Collections.Specialized.StringCollection" /> of property names.</returns>
	public StringCollection PropertiesNames
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.PrintSystemObjectPropertiesChangedEventArgs" /> class.</summary>
	/// <param name="events">A collection of strings, each of which identifies a changed property.</param>
	public PrintSystemObjectPropertiesChangedEventArgs(StringCollection events)
	{
	}

	/// <summary>Releases all resources used by the <see cref="T:System.Printing.PrintSystemObjectPropertiesChangedEventArgs" /> object.</summary>
	public void Dispose()
	{
	}

	/// <summary>Releases the unmanaged resources that are used by the <see cref="T:System.Printing.PrintSystemObjectPropertiesChangedEventArgs" /> object and optionally releases the managed resources.</summary>
	/// <param name="A_0">
	///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
	protected virtual void Dispose(bool A_0)
	{
	}
}
