using System;
using System.Runtime.InteropServices;

namespace MS.Internal.PrintWin32Thunk;

internal class SafeMemoryHandle : SafeHandle
{
	public override bool IsInvalid
	{
		get
		{
			throw null;
		}
	}

	public static SafeMemoryHandle Null
	{
		get
		{
			throw null;
		}
	}

	public virtual int Size
	{
		get
		{
			throw null;
		}
	}

	public SafeMemoryHandle(IntPtr Win32Pointer)
		: base((IntPtr)0, ownsHandle: false)
	{
	}

	public void CopyFromArray(byte[] source, int startIndex, int length)
	{
	}

	public void CopyToArray(byte[] destination, int startIndex, int length)
	{
	}

	public static SafeMemoryHandle Create(int byteCount)
	{
		throw null;
	}

	protected override bool ReleaseHandle()
	{
		throw null;
	}

	public static bool TryCreate(int byteCount, ref SafeMemoryHandle result)
	{
		throw null;
	}

	public static SafeMemoryHandle Wrap(IntPtr Win32Pointer)
	{
		throw null;
	}
}
