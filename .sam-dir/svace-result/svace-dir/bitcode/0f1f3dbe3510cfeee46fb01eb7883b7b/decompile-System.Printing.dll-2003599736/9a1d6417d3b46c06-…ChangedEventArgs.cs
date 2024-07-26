namespace System.Printing;

/// <summary>Provides data for a PropertyChanged event, which you must create.</summary>
public class PrintSystemObjectPropertyChangedEventArgs : EventArgs, IDisposable
{
	/// <summary>Gets the name of the property that changed.</summary>
	/// <returns>A <see cref="T:System.String" /> that holds the property name.</returns>
	public string PropertyName
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.PrintSystemObjectPropertyChangedEventArgs" /> class.</summary>
	/// <param name="eventName">The name of the property that changed.</param>
	public PrintSystemObjectPropertyChangedEventArgs(string eventName)
	{
	}

	/// <summary>Releases all resources used by the <see cref="T:System.Printing.PrintSystemObjectPropertyChangedEventArgs" />.</summary>
	public void Dispose()
	{
	}

	/// <summary>Releases the unmanaged resources that are used by the <see cref="T:System.Printing.PrintSystemObjectPropertyChangedEventArgs" /> and optionally releases the managed resources.</summary>
	/// <param name="A_0">
	///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
	protected virtual void Dispose(bool A_0)
	{
	}
}
