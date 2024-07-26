using System.Collections;
using System.Collections.Generic;

namespace System.Printing;

/// <summary>Represents a collection of <see cref="T:System.Printing.PrintQueue" /> objects.</summary>
public class PrintQueueCollection : PrintSystemObjects, IEnumerable<PrintQueue>, IEnumerable, IDisposable
{
	/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Printing.PrintQueueCollection" />.</summary>
	/// <returns>A <see cref="T:System.Object" /> that can be used to synchronize access to the collection.</returns>
	public static object SyncRoot
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.PrintQueueCollection" /> class.</summary>
	public PrintQueueCollection()
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.PrintQueueCollection" /> class for the specified <see cref="T:System.Printing.PrintServer" />.</summary>
	/// <param name="printServer">The print server that hosts the collection.</param>
	/// <param name="propertyFilter">The properties of the collection members that are initialized.</param>
	public PrintQueueCollection(PrintServer printServer, string[] propertyFilter)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.PrintQueueCollection" /> class for the specified <see cref="T:System.Printing.PrintServer" />, containing only the print server's queues of the specified <see cref="T:System.Printing.EnumeratedPrintQueueTypes" />.</summary>
	/// <param name="printServer">The print server that hosts the collection.</param>
	/// <param name="propertyFilter">The properties of the collection members that are initialized.</param>
	/// <param name="enumerationFlag">An array that specifies the types of print queues that are included in the collection.</param>
	public PrintQueueCollection(PrintServer printServer, string[] propertyFilter, EnumeratedPrintQueueTypes[] enumerationFlag)
	{
	}

	/// <summary>Adds a <see cref="T:System.Printing.PrintQueue" /> to the collection.</summary>
	/// <param name="printObject">The print queue that is added.</param>
	public void Add(PrintQueue printObject)
	{
	}

	/// <summary>Releases the unmanaged resources that are being used by the <see cref="T:System.Printing.PrintQueueCollection" />, and optionally releases the managed resources that are being used.</summary>
	/// <param name="A_0">
	///   <see langword="true" /> to release both the managed resources and the unmanaged resources; <see langword="false" /> to release only the unmanaged resources.</param>
	protected override void Dispose(bool A_0)
	{
	}

	/// <summary>Returns an object that implements the generic <see cref="T:System.Collections.IEnumerator" /> interface that has been closed with <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <returns>An object that implements the generic <see cref="T:System.Collections.IEnumerator" /> and that can iterate through the <see cref="T:System.Printing.PrintQueue" /> objects that the <see cref="T:System.Printing.PrintQueueCollection" /> contains.</returns>
	public virtual IEnumerator<PrintQueue> GetEnumerator()
	{
		throw null;
	}

	/// <summary>Gets an object that implements the non-generic <see cref="T:System.Collections.IEnumerator" /> interface.</summary>
	/// <returns>An object that implements the non-generic <see cref="T:System.Collections.IEnumerator" /> and that can iterate through the <see cref="T:System.Printing.PrintQueue" /> objects that the <see cref="T:System.Printing.PrintQueueCollection" /> contains.</returns>
	public virtual IEnumerator GetNonGenericEnumerator()
	{
		throw null;
	}

	/// <summary>Returns an object that implements the generic <see cref="T:System.Collections.IEnumerator" /> interface that has been closed with <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <returns>An object that implements the generic <see cref="T:System.Collections.IEnumerator" /> and that can iterate through the <see cref="T:System.Printing.PrintQueue" /> objects that the <see cref="T:System.Printing.PrintQueueCollection" /> contains.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
