namespace System.Printing;

/// <summary>Specifies the size of the paper (or other media), the size of the imageable area, and the location of the imageable area.</summary>
public class PrintDocumentImageableArea
{
	/// <summary>Gets the height of the imageable area.</summary>
	/// <returns>A <see cref="T:System.Double" /> that represents the distance from the origin.</returns>
	public double ExtentHeight
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets the width of the imageable area.</summary>
	/// <returns>A <see cref="T:System.Double" /> that represents the distance from the origin.</returns>
	public double ExtentWidth
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets the height of the paper or media.</summary>
	/// <returns>A <see cref="T:System.Double" /> that represents the distance from the upper-left corner of the page to the lower-left corner.</returns>
	public double MediaSizeHeight
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets the width of the paper or media.</summary>
	/// <returns>A <see cref="T:System.Double" /> that represents the distance from the upper-left corner of the page to the upper-right corner.</returns>
	public double MediaSizeWidth
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets the distance from the upper-left corner of the imageable area (also called the 'origin' of the imageable area) to the nearest point on the top edge of the page.</summary>
	/// <returns>A <see cref="T:System.Double" /> that represents the distance (in pixels - 1/96 of an inch) from the upper-left corner of the imageable area (also called the 'origin' of the imageable area) to the nearest point on the top edge of the page.</returns>
	public double OriginHeight
	{
		get
		{
			throw null;
		}
	}

	/// <summary>Gets the origin width, which is the distance from the left edge of the page to the upper-left corner of the imageable area (also called the "origin" of the imageable area).</summary>
	/// <returns>A <see cref="T:System.Double" /> that represents the origin width (in pixels - 1/96 of an inch), which is the distance from the left edge of the page to the upper-left corner of the imageable area (also called the "origin" of the imageable area).</returns>
	public double OriginWidth
	{
		get
		{
			throw null;
		}
	}

	internal PrintDocumentImageableArea()
	{
	}
}
