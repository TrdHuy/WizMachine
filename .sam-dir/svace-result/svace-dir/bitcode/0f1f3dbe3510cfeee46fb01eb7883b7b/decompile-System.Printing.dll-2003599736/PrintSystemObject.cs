using System.Printing.IndexedProperties;

namespace System.Printing;

/// <summary>Defines basic properties and methods that are common to the objects of the printing system. Classes that derive from this class represent such objects as print queues, print servers, and print jobs.</summary>
public abstract class PrintSystemObject : IDisposable
{
	/// <summary>Gets or sets a value that indicates whether the object has been disposed.</summary>
	/// <returns>
	///   <see langword="true" /> if the object has been disposed; otherwise <see langword="false" />.</returns>
	protected bool IsDisposed
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Gets the name of the object.</summary>
	/// <returns>A <see cref="T:System.String" /> that represents the name of the object.</returns>
	public virtual string Name
	{
		get
		{
			throw null;
		}
		internal set
		{
		}
	}

	/// <summary>Gets the parent of the object.</summary>
	/// <returns>Another <see cref="T:System.Printing.PrintSystemObject" />.</returns>
	public virtual PrintSystemObject Parent
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a collection of attribute and value pairs.</summary>
	/// <returns>A <see cref="T:System.Printing.IndexedProperties.PrintPropertyDictionary" /> that contains attribute and value pairs.</returns>
	public PrintPropertyDictionary PropertiesCollection
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.PrintSystemObject" /> class.</summary>
	protected PrintSystemObject()
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.PrintSystemObject" /> class by using the specified <see cref="T:System.Printing.PrintSystemObjectLoadMode" />.</summary>
	/// <param name="mode">A value that specifies whether the properties of the object should be initialized when the object is loaded.</param>
	protected PrintSystemObject(PrintSystemObjectLoadMode mode)
	{
	}

	/// <summary>Gets the names of the attributes of the derived class.</summary>
	/// <returns>An array of <see cref="T:System.String" /> values.</returns>
	protected static string[] BaseAttributeNames()
	{
		throw null;
	}

	/// <summary>When overridden in a derived class, writes any changes that your program has made to the object's properties to the actual software or hardware component that the object represents.</summary>
	public abstract void Commit();

	/// <summary>Releases all resources used by the <see cref="T:System.Printing.PrintSystemObject" />.</summary>
	public void Dispose()
	{
	}

	/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Printing.PrintSystemObject" /> and optionally releases the managed resources.</summary>
	/// <param name="A_0">
	///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
	protected virtual void Dispose(bool A_0)
	{
	}

	/// <summary>Releases the resources that are being used by the <see cref="T:System.Printing.PrintSystemObject" />.</summary>
	~PrintSystemObject()
	{
	}

	/// <summary>Initializes the properties of the <see cref="T:System.Printing.PrintSystemObject" />.</summary>
	protected void Initialize()
	{
	}

	/// <summary>When overridden in a derived class, releases the unmanaged resources that are being used by the <see cref="T:System.Printing.PrintSystemObject" />, and optionally releases the managed resources that are being used.</summary>
	/// <param name="disposing">
	///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
	protected virtual void InternalDispose(bool disposing)
	{
	}

	/// <summary>When overridden in a derived class, updates the properties of an object of the derived class so that its values match the values of the actual software or hardware component that the object represents.</summary>
	public abstract void Refresh();
}
