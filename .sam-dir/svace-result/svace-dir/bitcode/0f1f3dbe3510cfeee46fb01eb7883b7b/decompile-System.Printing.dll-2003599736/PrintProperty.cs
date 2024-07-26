using System.Runtime.Serialization;

namespace System.Printing.IndexedProperties;

/// <summary>Represents a property (and the value of the property) of a printing system hardware or software component.</summary>
public abstract class PrintProperty : IDisposable, IDeserializationCallback
{
	/// <summary>Gets or sets a value that indicates whether the object has been disposed.</summary>
	/// <returns>
	///   <see langword="true" /> if the object has been disposed; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
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

	/// <summary>Gets or sets a value that indicates whether the object has been initialized.</summary>
	/// <returns>
	///   <see langword="true" /> if the object has been initialized; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
	protected internal bool IsInitialized
	{
		get
		{
			throw null;
		}
		protected set
		{
		}
	}

	/// <summary>When overridden in a derived class, gets the name of the property that the object represents.</summary>
	/// <returns>A <see cref="T:System.String" /> that represents the name of the property.</returns>
	public virtual string Name
	{
		get
		{
			throw null;
		}
	}

	/// <summary>When overridden in a derived class, gets or sets the value of the property that the object represents.</summary>
	public abstract object Value { get; set; }

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintProperty" /> class.</summary>
	/// <param name="attributeName">The name of the property that this object represents.</param>
	protected PrintProperty(string attributeName)
	{
	}

	/// <summary>Releases all resources that are being used by the <see cref="T:System.Printing.IndexedProperties.PrintProperty" />.</summary>
	public void Dispose()
	{
	}

	/// <summary>Releases the unmanaged resources that are being used by the <see cref="T:System.Printing.IndexedProperties.PrintProperty" /> and optionally releases the managed resources.</summary>
	/// <param name="A_0">
	///   <see langword="true" /> to release both managed resources and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
	protected virtual void Dispose(bool A_0)
	{
	}

	/// <summary>Enables a <see cref="T:System.Printing.IndexedProperties.PrintProperty" /> to attempt to free resources and to perform other cleanup operations before the <see cref="T:System.Printing.IndexedProperties.PrintProperty" /> is reclaimed by garbage collection.</summary>
	~PrintProperty()
	{
	}

	/// <summary>Releases the unmanaged resources that are being used by the <see cref="T:System.Printing.IndexedProperties.PrintProperty" /> and optionally releases the managed resources.</summary>
	/// <param name="disposing">
	///   <see langword="true" /> to release both the managed resources and the unmanaged resources; <see langword="false" /> to release only the unmanaged resources.</param>
	protected virtual void InternalDispose(bool disposing)
	{
	}

	/// <summary>When overridden in a derived class, implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and raises the deserialization event when the deserialization is complete.</summary>
	/// <param name="sender">The source of the event.</param>
	public virtual void OnDeserialization(object sender)
	{
	}
}
