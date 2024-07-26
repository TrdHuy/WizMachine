namespace System.Printing;

/// <summary>Represents a print driver.</summary>
public sealed class PrintDriver : PrintFilter
{
	internal PrintDriver()
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
