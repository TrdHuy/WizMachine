using System.Runtime.Serialization;

namespace System.Windows.Xps;

/// <summary>The exception that is thrown when a method of either an <see cref="T:System.Windows.Xps.XpsDocumentWriter" /> or a <see cref="T:System.Windows.Xps.VisualsToXpsDocument" /> object is called that is incompatible with the current state of the object.</summary>
public class XpsWriterException : Exception
{
	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Xps.XpsWriterException" /> class.</summary>
	public XpsWriterException()
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Xps.XpsWriterException" /> class that provides specific <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" />. This constructor is protected.</summary>
	/// <param name="info">The data that is required to serialize or deserialize an object.</param>
	/// <param name="context">The context, which includes source and destination, of the serialized stream.</param>
	protected XpsWriterException(SerializationInfo info, StreamingContext context)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Xps.XpsWriterException" /> class that provides a specific error condition.</summary>
	/// <param name="message">A <see cref="T:System.String" /> that describes the error condition.</param>
	public XpsWriterException(string message)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Xps.XpsWriterException" /> class that provides a specific error condition and includes the cause of the exception.</summary>
	/// <param name="message">A <see cref="T:System.String" /> that describes the error condition.</param>
	/// <param name="innerException">The underlying error that caused the <see cref="T:System.Windows.Xps.XpsWriterException" />.</param>
	public XpsWriterException(string message, Exception innerException)
	{
	}
}
