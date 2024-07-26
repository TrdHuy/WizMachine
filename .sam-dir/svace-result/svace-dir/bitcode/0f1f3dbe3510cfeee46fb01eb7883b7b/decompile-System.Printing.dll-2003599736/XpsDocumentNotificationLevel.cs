namespace System.Windows.Xps;

/// <summary>Indicates whether a write operation to an XML Paper Specification (XPS) document or a print queue sends back page-by-page and document-by-document progress notifications.</summary>
public enum XpsDocumentNotificationLevel
{
	/// <summary>The notification status is not indicated.</summary>
	None = 0,
	/// <summary>Progress notifications are disabled.</summary>
	ReceiveNotificationDisabled = 2,
	/// <summary>Progress notifications are enabled.</summary>
	ReceiveNotificationEnabled = 1
}
