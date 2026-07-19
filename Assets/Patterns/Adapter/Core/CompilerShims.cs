// Unity's netstandard 2.1 profile compiles C# 9 but ships without the
// IsExternalInit marker type that init-only setters compile against.
// Declaring it ourselves (internal, per assembly) enables `init` properties.
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit
    {
    }
}
