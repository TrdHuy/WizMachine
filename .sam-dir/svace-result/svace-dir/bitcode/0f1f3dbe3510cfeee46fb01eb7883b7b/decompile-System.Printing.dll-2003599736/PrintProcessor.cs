namespace System.Printing;

/// <summary>Represents a print processor on a print server.</summary>
public sealed class PrintProcessor : PrintFilter
{
	internal PrintProcessor()
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
