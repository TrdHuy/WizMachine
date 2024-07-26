namespace System.Printing;

/// <summary>Represents a printer port on a print server. Each print queue has a print port assigned to it.</summary>
public sealed class PrintPort : PrintSystemObject
{
	internal PrintPort()
	{
	}

	/// <summary>Do not use.</summary>
	public sealed override void Commit()
	{
	}

	protected sealed override void InternalDispose(bool disposing)
	{
	}

	/// <summary>Do not use.</summary>
	public sealed override void Refresh()
	{
	}
}
