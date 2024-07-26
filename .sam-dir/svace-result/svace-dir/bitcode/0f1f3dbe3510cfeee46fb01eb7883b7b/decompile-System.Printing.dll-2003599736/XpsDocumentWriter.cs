using System.Printing;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Media;
using System.Windows.Xps.Packaging;

namespace System.Windows.Xps;

/// <summary>Provides methods to write to an XPS document or print queue.</summary>
public class XpsDocumentWriter : SerializerWriter
{
	/// <summary>Occurs when a <see cref="Overload:System.Windows.Xps.XpsDocumentWriter.Write" /> or <see cref="Overload:System.Windows.Xps.XpsDocumentWriter.WriteAsync" /> operation is canceled.</summary>
	public override event WritingCancelledEventHandler WritingCancelled
	{
		add
		{
		}
		remove
		{
		}
	}

	/// <summary>Occurs when a write operation finishes.</summary>
	public override event WritingCompletedEventHandler WritingCompleted
	{
		add
		{
		}
		remove
		{
		}
	}

	/// <summary>Occurs just before a <see cref="Overload:System.Windows.Xps.XpsDocumentWriter.Write" /> or <see cref="Overload:System.Windows.Xps.XpsDocumentWriter.WriteAsync" /> method adds a <see cref="T:System.Printing.PrintTicket" /> to a document or print queue.</summary>
	public override event WritingPrintTicketRequiredEventHandler WritingPrintTicketRequired
	{
		add
		{
		}
		remove
		{
		}
	}

	/// <summary>Occurs when the <see cref="T:System.Windows.Xps.XpsDocumentWriter" /> updates its progress.</summary>
	public override event WritingProgressChangedEventHandler WritingProgressChanged
	{
		add
		{
		}
		remove
		{
		}
	}

	internal XpsDocumentWriter()
	{
	}

	/// <summary>Cancels the current <see cref="Overload:System.Windows.Xps.XpsDocumentWriter.WriteAsync" /> operation.</summary>
	/// <exception cref="T:System.Windows.Xps.XpsWriterException">No asynchronous write operation is in progress.</exception>
	public override void CancelAsync()
	{
	}

	/// <summary>Returns a <see cref="T:System.Windows.Xps.VisualsToXpsDocument" /> that can write <see cref="T:System.Windows.Media.Visual" /> objects to a document or print queue.</summary>
	/// <returns>The new <see cref="T:System.Windows.Xps.VisualsToXpsDocument" />.</returns>
	public override SerializerWriterCollator CreateVisualsCollator()
	{
		throw null;
	}

	/// <summary>Returns a <see cref="T:System.Windows.Xps.VisualsToXpsDocument" /> that can write <see cref="T:System.Windows.Media.Visual" /> objects with <see cref="T:System.Printing.PrintTicket" /> settings to an XPS document or print queue.</summary>
	/// <param name="documentSequencePrintTicket">A <see cref="T:System.Printing.PrintTicket" /> that specifies the default printing preferences for the document sequence.</param>
	/// <param name="documentPrintTicket">A <see cref="T:System.Printing.PrintTicket" /> that represents the default printing preferences for each document.</param>
	/// <returns>The new <see cref="T:System.Windows.Xps.VisualsToXpsDocument" /> that writes <see cref="T:System.Windows.Media.Visual" /> elements with <see cref="T:System.Printing.PrintTicket" /> settings to the <see cref="T:System.Windows.Xps.Packaging.XpsDocument" />.</returns>
	public override SerializerWriterCollator CreateVisualsCollator(PrintTicket documentSequencePrintTicket, PrintTicket documentPrintTicket)
	{
		throw null;
	}

	/// <summary>Raises the <see cref="E:System.Windows.Xps.XpsDocumentWriter.WritingCancelled" /> event.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="args">The event data.</param>
	public virtual void raise_WritingCancelled(object sender, WritingCancelledEventArgs args)
	{
	}

	/// <summary>Raises the <see cref="E:System.Windows.Xps.XpsDocumentWriter.WritingCompleted" /> event.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	public virtual void raise_WritingCompleted(object sender, WritingCompletedEventArgs e)
	{
	}

	/// <summary>Raises the <see cref="E:System.Windows.Xps.XpsDocumentWriter.WritingPrintTicketRequired" /> event.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	public virtual void raise_WritingPrintTicketRequired(object sender, WritingPrintTicketRequiredEventArgs e)
	{
	}

	/// <summary>Raises the <see cref="E:System.Windows.Xps.XpsDocumentWriter.WritingProgressChanged" /> event.</summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	public virtual void raise_WritingProgressChanged(object sender, WritingProgressChangedEventArgs e)
	{
	}

	/// <summary>Synchronously writes a specified XPS document to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="documentPath">The path of the source document.</param>
	public void Write(string documentPath)
	{
	}

	/// <summary>Writes synchronously a specified XPS document to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="documentPath">The path of the source document.</param>
	/// <param name="notificationLevel">An indication of whether notification is enabled.</param>
	public void Write(string documentPath, XpsDocumentNotificationLevel notificationLevel)
	{
	}

	/// <summary>Writes synchronously paginated content from a specified <see cref="T:System.Windows.Documents.DocumentPaginator" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="documentPaginator">An object that contains a pointer to unpaginated source material and also contains methods for paginating the material.</param>
	public override void Write(DocumentPaginator documentPaginator)
	{
	}

	/// <summary>Writes synchronously paginated content from a specified <see cref="T:System.Windows.Documents.DocumentPaginator" /> together with a <see cref="T:System.Printing.PrintTicket" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="documentPaginator">An object that contains a pointer to unpaginated source material and also contains methods for paginating the material.</param>
	/// <param name="printTicket">A <see cref="T:System.Printing.PrintTicket" /> that represents the default printing preferences for material.</param>
	public override void Write(DocumentPaginator documentPaginator, PrintTicket printTicket)
	{
	}

	/// <summary>Writes synchronously a specified <see cref="T:System.Windows.Documents.FixedDocument" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="fixedDocument">A document that is written to the <see cref="T:System.Windows.Xps.Packaging.XpsDocument" />.</param>
	public override void Write(FixedDocument fixedDocument)
	{
	}

	/// <summary>Writes synchronously a <see cref="T:System.Windows.Documents.FixedDocument" /> together with a <see cref="T:System.Printing.PrintTicket" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="fixedDocument">The document that is written.</param>
	/// <param name="printTicket">A <see cref="T:System.Printing.PrintTicket" /> that represents the default printing preferences for the document.</param>
	public override void Write(FixedDocument fixedDocument, PrintTicket printTicket)
	{
	}

	/// <summary>Writes synchronously a specified <see cref="T:System.Windows.Documents.FixedDocumentSequence" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="fixedDocumentSequence">A set of documents that is written to the <see cref="T:System.Windows.Xps.Packaging.XpsDocument" />.</param>
	public override void Write(FixedDocumentSequence fixedDocumentSequence)
	{
	}

	/// <summary>Writes synchronously a specified <see cref="T:System.Windows.Documents.FixedDocumentSequence" /> together with a <see cref="T:System.Printing.PrintTicket" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="fixedDocumentSequence">The set of documents that are written.</param>
	/// <param name="printTicket">A <see cref="T:System.Printing.PrintTicket" /> that represents the default printing preferences for the set of documents.</param>
	public override void Write(FixedDocumentSequence fixedDocumentSequence, PrintTicket printTicket)
	{
	}

	/// <summary>Writes synchronously a specified <see cref="T:System.Windows.Documents.FixedPage" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="fixedPage">A page that is written to the <see cref="T:System.Windows.Xps.Packaging.XpsDocument" />.</param>
	public override void Write(FixedPage fixedPage)
	{
	}

	/// <summary>Writes synchronously a specified <see cref="T:System.Windows.Documents.FixedPage" /> together with a <see cref="T:System.Printing.PrintTicket" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="fixedPage">The page that is written.</param>
	/// <param name="printTicket">A <see cref="T:System.Printing.PrintTicket" /> that represents the default printing preferences for the page.</param>
	public override void Write(FixedPage fixedPage, PrintTicket printTicket)
	{
	}

	/// <summary>Writes synchronously a specified <see cref="T:System.Windows.Media.Visual" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="visual">The <see cref="T:System.Windows.Media.Visual" /> that is written.</param>
	public override void Write(Visual visual)
	{
	}

	/// <summary>Writes synchronously a specified <see cref="T:System.Windows.Media.Visual" /> together with a <see cref="T:System.Printing.PrintTicket" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="visual">The <see cref="T:System.Windows.Media.Visual" /> that is written.</param>
	/// <param name="printTicket">A <see cref="T:System.Printing.PrintTicket" /> that represents the default printing preferences for the document.</param>
	public override void Write(Visual visual, PrintTicket printTicket)
	{
	}

	/// <summary>Writes asynchronously a specified XPS document to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="documentPath">The path of the source document.</param>
	public void WriteAsync(string documentPath)
	{
	}

	/// <summary>Writes asynchronously a specified XPS document with notification option to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="documentPath">The path of the source document.</param>
	/// <param name="notificationLevel">An indication of whether notification is enabled.</param>
	public void WriteAsync(string documentPath, XpsDocumentNotificationLevel notificationLevel)
	{
	}

	/// <summary>Writes asynchronously paginated content from a specified <see cref="T:System.Windows.Documents.DocumentPaginator" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="documentPaginator">An object that contains a pointer to unpaginated source material and also contains methods for paginating the material.</param>
	public override void WriteAsync(DocumentPaginator documentPaginator)
	{
	}

	/// <summary>Writes asynchronously paginated content from a specified <see cref="T:System.Windows.Documents.DocumentPaginator" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="documentPaginator">An object that contains a pointer to unpaginated source material; also contains methods for paginating the material.</param>
	/// <param name="userSuppliedState">A user-specified object to identify and associate with the asynchronous operation.</param>
	public override void WriteAsync(DocumentPaginator documentPaginator, object userSuppliedState)
	{
	}

	/// <summary>Writes asynchronously paginated content from a specified <see cref="T:System.Windows.Documents.DocumentPaginator" /> together with a <see cref="T:System.Printing.PrintTicket" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="documentPaginator">An object that contains a pointer to unpaginated source material and also contains methods for paginating the material.</param>
	/// <param name="printTicket">A <see cref="T:System.Printing.PrintTicket" /> that represents the default printing preferences for the material.</param>
	public override void WriteAsync(DocumentPaginator documentPaginator, PrintTicket printTicket)
	{
	}

	/// <summary>Writes asynchronously paginated content from a specified <see cref="T:System.Windows.Documents.DocumentPaginator" /> together with a <see cref="T:System.Printing.PrintTicket" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="documentPaginator">An object that contains a pointer to unpaginated source material; also contains methods for paginating the material.</param>
	/// <param name="printTicket">A <see cref="T:System.Printing.PrintTicket" /> that represents the default printing preferences for the material.</param>
	/// <param name="userSuppliedState">A user-specified object to identify and associate with the asynchronous operation.</param>
	public override void WriteAsync(DocumentPaginator documentPaginator, PrintTicket printTicket, object userSuppliedState)
	{
	}

	/// <summary>Writes asynchronously a specified <see cref="T:System.Windows.Documents.FixedDocument" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="fixedDocument">The document that is written.</param>
	public override void WriteAsync(FixedDocument fixedDocument)
	{
	}

	/// <summary>Writes asynchronously a specified <see cref="T:System.Windows.Documents.FixedDocument" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="fixedDocument">The document that is written.</param>
	/// <param name="userSuppliedState">A user-specified object to identify and associate with the asynchronous operation.</param>
	public override void WriteAsync(FixedDocument fixedDocument, object userSuppliedState)
	{
	}

	/// <summary>Writes asynchronously a specified <see cref="T:System.Windows.Documents.FixedDocument" /> together with a <see cref="T:System.Printing.PrintTicket" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="fixedDocument">The document that is written.</param>
	/// <param name="printTicket">A <see cref="T:System.Printing.PrintTicket" /> that represents the default printing preferences for the document.</param>
	public override void WriteAsync(FixedDocument fixedDocument, PrintTicket printTicket)
	{
	}

	/// <summary>Writes asynchronously a specified <see cref="T:System.Windows.Documents.FixedDocument" /> together with a <see cref="T:System.Printing.PrintTicket" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="fixedDocument">The document that is written.</param>
	/// <param name="printTicket">A <see cref="T:System.Printing.PrintTicket" /> that represents the default printing preferences for the document.</param>
	/// <param name="userSuppliedState">A user-specified object to identify and associate with the asynchronous operation.</param>
	public override void WriteAsync(FixedDocument fixedDocument, PrintTicket printTicket, object userSuppliedState)
	{
	}

	/// <summary>Writes asynchronously a specified <see cref="T:System.Windows.Documents.FixedDocumentSequence" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="fixedDocumentSequence">The set of documents that is written.</param>
	public override void WriteAsync(FixedDocumentSequence fixedDocumentSequence)
	{
	}

	/// <summary>Writes asynchronously a specified <see cref="T:System.Windows.Documents.FixedDocumentSequence" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="fixedDocumentSequence">The set of documents that are written.</param>
	/// <param name="userSuppliedState">A user-specified object to identify and associate with the asynchronous operation.</param>
	public override void WriteAsync(FixedDocumentSequence fixedDocumentSequence, object userSuppliedState)
	{
	}

	/// <summary>Writes asynchronously a specified <see cref="T:System.Windows.Documents.FixedDocumentSequence" /> together with a <see cref="T:System.Printing.PrintTicket" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="fixedDocumentSequence">The set of documents that are written.</param>
	/// <param name="printTicket">A <see cref="T:System.Printing.PrintTicket" /> that represents the default printing preferences for the set of documents.</param>
	public override void WriteAsync(FixedDocumentSequence fixedDocumentSequence, PrintTicket printTicket)
	{
	}

	/// <summary>Writes asynchronously a specified <see cref="T:System.Windows.Documents.FixedDocumentSequence" /> together with a <see cref="T:System.Printing.PrintTicket" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="fixedDocumentSequence">The set of documents to be written.</param>
	/// <param name="printTicket">A <see cref="T:System.Printing.PrintTicket" /> that represents the default printing preferences for the set of documents.</param>
	/// <param name="userSuppliedState">A user-specified object to identify and associate with the asynchronous operation.</param>
	public override void WriteAsync(FixedDocumentSequence fixedDocumentSequence, PrintTicket printTicket, object userSuppliedState)
	{
	}

	/// <summary>Writes asynchronously a specified <see cref="T:System.Windows.Documents.FixedPage" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="fixedPage">The page that is written.</param>
	public override void WriteAsync(FixedPage fixedPage)
	{
	}

	/// <summary>Writes asynchronously a specified <see cref="T:System.Windows.Documents.FixedPage" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="fixedPage">The page that is written.</param>
	/// <param name="userSuppliedState">A user-specified object to identify and associate with the asynchronous operation.</param>
	public override void WriteAsync(FixedPage fixedPage, object userSuppliedState)
	{
	}

	/// <summary>Writes asynchronously a specified <see cref="T:System.Windows.Documents.FixedPage" /> together with a <see cref="T:System.Printing.PrintTicket" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="fixedPage">The page that is written.</param>
	/// <param name="printTicket">A <see cref="T:System.Printing.PrintTicket" /> that represents the default printing preferences for the page.</param>
	public override void WriteAsync(FixedPage fixedPage, PrintTicket printTicket)
	{
	}

	/// <summary>Writes asynchronously a specified <see cref="T:System.Windows.Documents.FixedPage" /> together with a <see cref="T:System.Printing.PrintTicket" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="fixedPage">The page that is written.</param>
	/// <param name="printTicket">A <see cref="T:System.Printing.PrintTicket" /> that represents the default printing preferences for the page.</param>
	/// <param name="userSuppliedState">A user-specified object to identify and associate with the asynchronous operation.</param>
	public override void WriteAsync(FixedPage fixedPage, PrintTicket printTicket, object userSuppliedState)
	{
	}

	/// <summary>Writes asynchronously a specified <see cref="T:System.Windows.Media.Visual" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="visual">The <see cref="T:System.Windows.Media.Visual" /> that is written.</param>
	public override void WriteAsync(Visual visual)
	{
	}

	/// <summary>Writes asynchronously a specified <see cref="T:System.Windows.Media.Visual" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="visual">The <see cref="T:System.Windows.Media.Visual" /> that is written.</param>
	/// <param name="userSuppliedState">A user-specified object to identify and associate with the asynchronous operation.</param>
	public override void WriteAsync(Visual visual, object userSuppliedState)
	{
	}

	/// <summary>Writes asynchronously a specified <see cref="T:System.Windows.Media.Visual" /> together with a <see cref="T:System.Printing.PrintTicket" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="visual">The <see cref="T:System.Windows.Media.Visual" /> that is written.</param>
	/// <param name="printTicket">A <see cref="T:System.Printing.PrintTicket" /> that represents the default printing preferences for the document.</param>
	public override void WriteAsync(Visual visual, PrintTicket printTicket)
	{
	}

	/// <summary>Writes asynchronously a specified <see cref="T:System.Windows.Media.Visual" /> together with a <see cref="T:System.Printing.PrintTicket" /> to the target <see cref="T:System.Windows.Xps.Packaging.XpsDocument" /> or <see cref="T:System.Printing.PrintQueue" />.</summary>
	/// <param name="visual">The <see cref="T:System.Windows.Media.Visual" /> that is written.</param>
	/// <param name="printTicket">A <see cref="T:System.Printing.PrintTicket" /> that represents the default printing preferences for the document.</param>
	/// <param name="userSuppliedState">A user-specified object to identify and associate with the asynchronous operation.</param>
	public override void WriteAsync(Visual visual, PrintTicket printTicket, object userSuppliedState)
	{
	}

	internal XpsDocumentWriter(PrintQueue printQueue)
	{
	}

	internal XpsDocumentWriter(XpsDocument document)
	{
	}
}
