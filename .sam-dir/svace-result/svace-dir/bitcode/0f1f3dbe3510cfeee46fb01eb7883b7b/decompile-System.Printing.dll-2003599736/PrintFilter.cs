namespace System.Printing;

/// <summary>Defines disposal behavior that is common to both the <see cref="T:System.Printing.PrintDriver" /> and <see cref="T:System.Printing.PrintProcessor" /> classes. <see cref="T:System.Printing.PrintFilter" /> supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
public abstract class PrintFilter : PrintSystemObject
{
	internal PrintFilter()
	{
	}

	/// <summary>Releases the unmanaged resources that are used by the class that is derived from <see cref="T:System.Printing.PrintFilter" /> and optionally releases the managed resources.</summary>
	/// <param name="disposing">
	///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
	protected override void InternalDispose(bool disposing)
	{
	}
}
