// Enables C# 9 init-only setters in this assembly on Unity's netstandard 2.1
// profile, which compiles C# 9 but ships without the IsExternalInit marker.
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit
    {
    }
}
