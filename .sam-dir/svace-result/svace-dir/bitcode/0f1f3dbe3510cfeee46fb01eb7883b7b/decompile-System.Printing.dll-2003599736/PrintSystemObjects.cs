namespace System.Printing;

/// <summary>Represents a collection of print system objects.</summary>
public abstract class PrintSystemObjects : IDisposable
{
	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.PrintSystemObjects" /> class.</summary>
	protected PrintSystemObjects()
	{
	}

	/// <summary>Releases all resources used by the <see cref="T:System.Printing.PrintSystemObjects" />.</summary>
	public void Dispose()
	{
	}

	/// <summary>Releases the unmanaged resources that are used by the <see cref="T:System.Printing.PrintSystemObjects" /> and optionally releases the managed resources.</summary>
	/// <param name="A_0">
	///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
	protected virtual void Dispose(bool A_0)
	{
	}
}
