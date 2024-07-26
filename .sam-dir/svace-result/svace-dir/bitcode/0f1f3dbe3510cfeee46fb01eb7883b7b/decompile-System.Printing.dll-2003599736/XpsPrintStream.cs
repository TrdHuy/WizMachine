using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace MS.Internal.PrintWin32Thunk;

internal class XpsPrintStream : Stream
{
	public override bool CanRead
	{
		get
		{
			throw null;
		}
	}

	public override bool CanSeek
	{
		get
		{
			throw null;
		}
	}

	public override bool CanTimeout
	{
		get
		{
			throw null;
		}
	}

	public override bool CanWrite
	{
		get
		{
			throw null;
		}
	}

	public override long Length
	{
		get
		{
			throw null;
		}
	}

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

	public static XpsPrintStream CreateXpsPrintStream()
	{
		throw null;
	}

	protected override void Dispose(bool A_0)
	{
	}

	public override void Flush()
	{
	}

	public IStream GetManagedIStream()
	{
		throw null;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		throw null;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw null;
	}

	public override void SetLength(long value)
	{
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
	}
}
