namespace System.Printing;

/// <summary>Specifies whether the properties of an object are initialized when the object loads.</summary>
public enum PrintSystemObjectLoadMode
{
	/// <summary>The properties are initialized during loading.</summary>
	LoadInitialized = 2,
	/// <summary>The properties are not initialized during loading.</summary>
	LoadUninitialized = 1,
	/// <summary>Not specified whether the properties are initialized.</summary>
	None = 0
}
