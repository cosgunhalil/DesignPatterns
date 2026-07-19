// Enables C# 9 init-only setters in this assembly on Unity's netstandard 2.1
// profile. The marker type must exist in every assembly that DECLARES init
// properties (Character lives here), and it is internal per assembly.
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit
    {
    }
}
