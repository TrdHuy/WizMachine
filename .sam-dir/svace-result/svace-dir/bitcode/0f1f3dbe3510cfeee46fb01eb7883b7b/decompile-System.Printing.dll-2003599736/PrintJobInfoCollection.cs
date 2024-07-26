using System.Collections;
using System.Collections.Generic;

namespace System.Printing;

/// <summary>Represents one or more <see cref="T:System.Printing.PrintSystemJobInfo" /> objects.</summary>
public class PrintJobInfoCollection : PrintSystemObjects, IEnumerable<PrintSystemJobInfo>, IEnumerable, IDisposable
{
	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.PrintJobInfoCollection" /> class that contains the <see cref="T:System.Printing.PrintSystemJobInfo" /> objects for every job that is in the specified <see cref="T:System.Printing.PrintQueue" /> and that initializes those objects only in the properties that are listed in the specified property filter.</summary>
	/// <param name="printQueue">The print queue whose print jobs will populate the collection.</param>
	/// <param name="propertyFilter">A list of a subset of the properties of a <see cref="T:System.Printing.PrintSystemJobInfo" /> object.</param>
	public PrintJobInfoCollection(PrintQueue printQueue, string[] propertyFilter)
	{
	}

	/// <summary>Adds a member to the <see cref="T:System.Printing.PrintJobInfoCollection" />.</summary>
	/// <param name="printObject">The object that is added.</param>
	public void Add(PrintSystemJobInfo printObject)
	{
	}

	/// <summary>Releases the unmanaged resources that are being used by the <see cref="T:System.Printing.PrintJobInfoCollection" /> and optionally releases the managed resources.</summary>
	/// <param name="A_0">
	///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
	protected override void Dispose(bool A_0)
	{
	}

	/// <summary>Gets an object that implements the generic <see cref="T:System.Collections.IEnumerator" /> interface that is closed with <see cref="T:System.Printing.PrintSystemJobInfo" />.</summary>
	/// <returns>An object that implements the generic <see cref="T:System.Collections.IEnumerator" /> interface and that can iterate through the <see cref="T:System.Printing.PrintSystemJobInfo" /> objects that the <see cref="T:System.Printing.PrintJobInfoCollection" /> contains.</returns>
	public virtual IEnumerator<PrintSystemJobInfo> GetEnumerator()
	{
		throw null;
	}

	/// <summary>Gets an object that implements the non-generic <see cref="T:System.Collections.IEnumerator" /> interface.</summary>
	/// <returns>An object that implements the non-generic <see cref="T:System.Collections.IEnumerator" /> interface and that can iterate through the <see cref="T:System.Printing.PrintSystemJobInfo" /> objects that the <see cref="T:System.Printing.PrintJobInfoCollection" /> contains.</returns>
	public virtual IEnumerator GetNonGenericEnumerator()
	{
		throw null;
	}

	/// <summary>Gets an object that implements the generic <see cref="T:System.Collections.IEnumerator" /> interface that is closed with <see cref="T:System.Printing.PrintSystemJobInfo" />.</summary>
	/// <returns>An object that implements the generic <see cref="T:System.Collections.IEnumerator" /> interface and that can iterate through the <see cref="T:System.Printing.PrintSystemJobInfo" /> objects that the <see cref="T:System.Printing.PrintJobInfoCollection" /> contains.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		throw null;
	}
}
