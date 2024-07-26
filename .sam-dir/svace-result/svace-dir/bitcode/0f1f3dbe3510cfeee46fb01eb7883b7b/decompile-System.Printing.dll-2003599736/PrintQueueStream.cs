using System.IO;
using System.Windows.Xps.Packaging;

namespace System.Printing;

/// <summary>A stream that represents a spooled print job in a print queue.</summary>
public class PrintQueueStream : Stream
{
	/// <summary>Gets a value that indicates whether the stream supports reading.</summary>
	/// <returns>
	///   <see langword="true" /> if reading is supported; otherwise <see langword="false" />.</returns>
	public override bool CanRead
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the stream supports seeking, which is moving the read/write position to a new position in the stream.</summary>
	/// <returns>
	///   <see langword="true" /> if seeking is supported; otherwise <see langword="false" />.</returns>
	public override bool CanSeek
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets a value that indicates whether the stream supports writing.</summary>
	/// <returns>
	///   <see langword="true" /> if writing is supported; otherwise <see langword="false" />.</returns>
	public override bool CanWrite
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets the ID number of the print job.</summary>
	/// <returns>An <see cref="T:System.Int32" /> that represents an ID number.</returns>
	public int JobIdentifier
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets the length of the stream in bytes.</summary>
	/// <returns>An <see cref="T:System.Int64" /> that represents the length of the stream in bytes.</returns>
	public override long Length
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets or sets the current read/write position in the stream.</summary>
	/// <returns>An <see cref="T:System.Int64" /> that represents the current position in the stream.</returns>
	public override long Position
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.PrintQueueStream" /> class for the specified print job that is hosted in the specified <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="printQueue">The <see cref="T:System.Printing.PrintQueue" /> that hosts the print job that provides the content of the stream.</param>
	/// <param name="printJobName">The name of the print job that provides the content of the stream.</param>
	public PrintQueueStream(PrintQueue printQueue, string printJobName)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.PrintQueueStream" /> class for the specified print job that is hosted in the specified <see cref="T:System.Printing.PrintQueue" />, with an indication of whether data in the <see cref="T:System.Printing.PrintQueueStream" /> should be committed when the stream is closed.</summary>
	/// <param name="printQueue">The <see cref="T:System.Printing.PrintQueue" /> that hosts the print job that provides the content of the stream.</param>
	/// <param name="printJobName">The name of the print job that provides the content of the stream.</param>
	/// <param name="commitDataOnClose">
	///   <see langword="true" /> to commit data in the <see cref="T:System.Printing.PrintQueueStream" /> when the <see cref="M:System.Printing.PrintQueueStream.Close" /> method is called; otherwise, <see langword="false" />.</param>
	public PrintQueueStream(PrintQueue printQueue, string printJobName, bool commitDataOnClose)
	{
	}

	/// <summary>Initializes a new instance of the <see cref="T:System.Printing.PrintQueueStream" /> class for the specified print job that is hosted in the specified <see cref="T:System.Printing.PrintQueue" />, with the specified settings and an indication of whether data in the <see cref="T:System.Printing.PrintQueueStream" /> should be committed when the stream is closed.</summary>
	/// <param name="printQueue">The <see cref="T:System.Printing.PrintQueue" /> that hosts the print job that provides the content of the stream.</param>
	/// <param name="printJobName">The name of the print job that provides the content of the stream.</param>
	/// <param name="commitDataOnClose">
	///   <see langword="true" /> to commit data in the <see cref="T:System.Printing.PrintQueueStream" /> when the <see cref="M:System.Printing.PrintQueueStream.Close" /> method is called; otherwise, <see langword="false" />.</param>
	/// <param name="printTicket">The settings of the print job.</param>
	public PrintQueueStream(PrintQueue printQueue, string printJobName, bool commitDataOnClose, PrintTicket printTicket)
	{
	}

	/// <summary>Begins an asynchronous write operation.</summary>
	/// <param name="buffer">The buffer from which to write data.</param>
	/// <param name="offset">The byte offset in the buffer from which to begin writing.</param>
	/// <param name="count">The maximum number of bytes to write.</param>
	/// <param name="callback">An asynchronous callback, which is called when the writing operation is complete.</param>
	/// <param name="state">A user-provided object that distinguishes this asynchronous writing request from other requests.</param>
	/// <returns>An <see cref="T:System.IAsyncResult" /> that represents the asynchronous write, which might still be pending.</returns>
	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		throw null;
	}

	/// <summary>Closes the stream and releases any resources, such as sockets and file handles, that are associated with it.</summary>
	public override void Close()
	{
	}

	/// <summary>Releases the unmanaged resources that are used by the <see cref="T:System.Printing.PrintQueueStream" /> and optionally releases the managed resources.</summary>
	/// <param name="A_0">
	///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
	protected override void Dispose(bool A_0)
	{
	}

	/// <summary>Ends an asynchronous write operation.</summary>
	/// <param name="asyncResult">A reference to the pending asynchronous I/O request.</param>
	public override void EndWrite(IAsyncResult asyncResult)
	{
	}

	/// <summary>Enables a <see cref="T:System.Printing.PrintQueueStream" /> to attempt to free resources and perform other cleanup operations before the <see cref="T:System.Printing.PrintQueueStream" /> is reclaimed by garbage collection.</summary>
	~PrintQueueStream()
	{
	}

	/// <summary>Clears all the buffers for this stream and writes any buffered data to the underlying device.</summary>
	public override void Flush()
	{
	}

	/// <summary>Enables the <see cref="T:System.Printing.PrintQueueStream" /> to respond to the packaging progress by handling the <see cref="E:System.Windows.Xps.Serialization.XpsPackagingPolicy.PackagingProgressEvent" />.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	public void HandlePackagingProgressEvent(object sender, PackagingProgressEventArgs e)
	{
	}

	/// <summary>Reads a sequence of bytes from the stream and advances the read/write position in the stream by the number of bytes that were read.</summary>
	/// <param name="buffer">An array of bytes.</param>
	/// <param name="offset">The zero-based byte offset in the buffer where you want to begin storing the data that is read from the stream.</param>
	/// <param name="count">The maximum number of bytes to be read from the stream.</param>
	/// <returns>An <see cref="T:System.Int32" /> that holds the total number of bytes that are read into the buffer.</returns>
	public override int Read(byte[] buffer, int offset, int count)
	{
		throw null;
	}

	/// <summary>Sets the read/write position within the stream.</summary>
	/// <param name="offset">A byte offset that is relative to the origin parameter.</param>
	/// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin" /> that indicates the reference point that is used to obtain the new position.</param>
	/// <returns>An <see cref="T:System.Int64" /> that represents the new read/write position.</returns>
	public override long Seek(long offset, SeekOrigin origin)
	{
		throw null;
	}

	/// <summary>Sets the length of the stream.</summary>
	/// <param name="value">The needed length, in bytes, of the current stream.</param>
	public override void SetLength(long value)
	{
	}

	/// <summary>Writes a sequence of bytes to the stream and advances the read/write position in the stream by the number of bytes that are written.</summary>
	/// <param name="buffer">An array of bytes from which to copy to the stream.</param>
	/// <param name="offset">The zero-based byte offset in the <paramref name="buffer" /> where you want to begin copying bytes to the stream.</param>
	/// <param name="count">The number of bytes to write to the stream.</param>
	public override void Write(byte[] buffer, int offset, int count)
	{
	}
}
