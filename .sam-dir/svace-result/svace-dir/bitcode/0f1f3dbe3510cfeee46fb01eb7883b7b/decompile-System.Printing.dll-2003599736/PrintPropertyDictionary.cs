using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;

namespace System.Printing.IndexedProperties;

/// <summary>Represents a collection of properties and values that are associated with an object in the <see cref="N:System.Printing" /> namespace.</summary>
[DefaultMember("Property")]
public class PrintPropertyDictionary : Hashtable, IDisposable, IDeserializationCallback, ISerializable
{
	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintPropertyDictionary" /> class.</summary>
	public PrintPropertyDictionary()
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.IndexedProperties.PrintPropertyDictionary" /> class and provides it with the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" />.</summary>
	/// <param name="info">The data that is required to serialize or deserialize an object.</param>
	/// <param name="context">The context of the serialized stream, including the source and the destination.</param>
	protected PrintPropertyDictionary(SerializationInfo info, StreamingContext context)
	{
	}

	/// <summary>Adds the specified object (of a class that is derived from <see cref="T:System.Printing.IndexedProperties.PrintProperty" />) into the dictionary.</summary>
	/// <param name="attributeValue">An object (of a class that is derived from <see cref="T:System.Printing.IndexedProperties.PrintProperty" />) that represents a property of a printing system hardware or software component.</param>
	public void Add(PrintProperty attributeValue)
	{
	}

	/// <summary>Releases all the resources that are being used by the <see cref="T:System.Printing.IndexedProperties.PrintPropertyDictionary" />.</summary>
	public void Dispose()
	{
	}

	/// <summary>Releases the unmanaged resources that are being used by the <see cref="T:System.Printing.IndexedProperties.PrintPropertyDictionary" /> and optionally releases the managed resources.</summary>
	/// <param name="A_0">
	///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
	protected virtual void Dispose(bool A_0)
	{
	}

	/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data that is needed to serialize the <see cref="T:System.Printing.IndexedProperties.PrintPropertyDictionary" />.</summary>
	/// <param name="info">Stores all the data that is used to serialize the object.</param>
	/// <param name="context">Describes the context of the serialized stream, including the source and the destination.</param>
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	}

	/// <summary>Gets the object (of a class that is derived from <see cref="T:System.Printing.IndexedProperties.PrintProperty" />) that represents the specified property.</summary>
	/// <param name="attribName">The name of the property that is represented by the <see cref="T:System.Printing.IndexedProperties.PrintProperty" />.</param>
	/// <returns>An object of a class that is derived from the <see cref="T:System.Printing.IndexedProperties.PrintProperty" />.</returns>
	public PrintProperty GetProperty(string attribName)
	{
		throw null;
	}

	/// <summary>Implements the <see cref="T:System.Runtime.Serialization.ISerializable" /> interface and raises the deserialization event when the deserialization is complete.</summary>
	/// <param name="sender">The source of the event.</param>
	public override void OnDeserialization(object sender)
	{
	}

	/// <summary>Sets the value of the specified attribute to an object of a class that is derived from <see cref="T:System.Printing.IndexedProperties.PrintProperty" />.</summary>
	/// <param name="attribName">The name of the attribute.</param>
	/// <param name="attribValue">An object of a type that is derived from <see cref="T:System.Printing.IndexedProperties.PrintProperty" />.</param>
	/// <exception cref="T:System.ArgumentException">
	///   <paramref name="attribName" /> is already in the dictionary and it already has the value <paramref name="attribValue" />.</exception>
	public void SetProperty(string attribName, PrintProperty attribValue)
	{
	}
}
