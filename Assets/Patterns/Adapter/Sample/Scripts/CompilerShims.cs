// Enables C# 9 init-only setters in this assembly on Unity's netstandard 2.1
// profile (the marker type must exist in every assembly that DECLARES init
// properties; consuming them needs nothing).
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit
    {
    }
}
