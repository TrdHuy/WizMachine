using System.Printing;
using System.Windows.Documents.Serialization;
using System.Windows.Media;

namespace System.Windows.Xps;

/// <summary>Provides methods for writing <see cref="T:System.Windows.Media.Visual" /> objects to XML Paper Specification (XPS) documents or to a print queue in batch mode.</summary>
public class VisualsToXpsDocument : SerializerWriterCollator
{
	internal VisualsToXpsDocument()
	{
	}

	/// <summary>Indicates that write operations can begin.</summary>
	public override void BeginBatchWrite()
	{
	}

	/// <summary>Cancels a synchronous writing operation.</summary>
	/// <exception cref="T:System.Windows.Xps.XpsWriterException">The state of the <see cref="T:System.Windows.Xps.VisualsToXpsDocument" /> is not compatible with a <see cref="M:System.Windows.Xps.VisualsToXpsDocument.Cancel" /> operation.</exception>
	public override void Cancel()
	{
	}

	/// <summary>Cancels an asynchronous writing operation.</summary>
	/// <exception cref="T:System.Windows.Xps.XpsWriterException">The state of the <see cref="T:System.Windows.Xps.VisualsToXpsDocument" /> is not compatible with a <see cref="M:System.Windows.Xps.VisualsToXpsDocument.CancelAsync" /> operation.</exception>
	public override void CancelAsync()
	{
	}

	/// <summary>Indicates that write operations must end.</summary>
	public override void EndBatchWrite()
	{
	}

	/// <summary>Writes a <see cref="T:System.Windows.Media.Visual" /> synchronously to an <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or a <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="visual">The <see cref="T:System.Windows.Media.Visual" /> that is written.</param>
	public override void Write(Visual visual)
	{
	}

	/// <summary>Writes a <see cref="T:System.Windows.Media.Visual" /> synchronously to an <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or a <see cref="T:System.Printing.PrintQueue" /> and includes a <see cref="T:System.Printing.PrintTicket" />.</summary>
	/// <param name="visual">The <see cref="T:System.Windows.Media.Visual" /> that is written.</param>
	/// <param name="printTicket">A <see cref="T:System.Printing.PrintTicket" /> that represents the default printing preferences for the document.</param>
	public override void Write(Visual visual, PrintTicket printTicket)
	{
	}

	/// <summary>Writes a <see cref="T:System.Windows.Media.Visual" /> asynchronously to an <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or a <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="visual">The <see cref="T:System.Windows.Media.Visual" /> that is written.</param>
	public override void WriteAsync(Visual visual)
	{
	}

	/// <summary>Writes a <see cref="T:System.Windows.Media.Visual" /> asynchronously to an <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or a <see cref="T:System.Printing.PrintQueue" /> and includes additional information that the caller wants to pass to an event handler.</summary>
	/// <param name="visual">The <see cref="T:System.Windows.Media.Visual" /> that is written.</param>
	/// <param name="userSuppliedState">An object that contains data that the caller wants to pass to the <see cref="E:System.Windows.Xps.XpsDocumentWriter.WritingCompleted" /> event handler.</param>
	public override void WriteAsync(Visual visual, object userSuppliedState)
	{
	}

	/// <summary>Writes a <see cref="T:System.Windows.Media.Visual" /> asynchronously to an <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or a <see cref="T:System.Printing.PrintQueue" /> and includes a <see cref="T:System.Printing.PrintTicket" />.</summary>
	/// <param name="visual">The <see cref="T:System.Windows.Media.Visual" /> that is written.</param>
	/// <param name="printTicket">A <see cref="T:System.Printing.PrintTicket" /> that represents the default printing preferences for the document.</param>
	public override void WriteAsync(Visual visual, PrintTicket printTicket)
	{
	}

	/// <summary>Writes a <see cref="T:System.Windows.Media.Visual" /> asynchronously to an <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or a <see cref="T:System.Printing.PrintQueue" />; also includes a <see cref="T:System.Printing.PrintTicket" /> and any additional information that the caller wants to pass to an event handler.</summary>
	/// <param name="visual">The <see cref="T:System.Windows.Media.Visual" /> that is written.</param>
	/// <param name="printTicket">A <see cref="T:System.Printing.PrintTicket" /> that represents the default printing preferences for the document.</param>
	/// <param name="userSuppliedState">An object that contains the data that the caller wants to pass to the <see cref="E:System.Windows.Xps.XpsDocumentWriter.WritingCompleted" /> event handler.</param>
	public override void WriteAsync(Visual visual, PrintTicket printTicket, object userSuppliedState)
	{
	}
}
