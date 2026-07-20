// Enables C# 9 init-only setters in this assembly on Unity's netstandard 2.1
// profile (HealthChanged uses init). The marker type is internal per assembly.
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit
    {
    }
}
