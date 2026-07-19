using System;
using System.Collections.Generic;

namespace DesignPatterns.Builder
{
    /// <summary>
    /// Thrown by <see cref="Builder{TSelf,TProduct}.Build"/> when the configured
    /// state is invalid. Carries every validation error at once, so a caller
    /// sees all problems rather than fixing them one exception at a time.
    /// </summary>
    public sealed class BuilderValidationException : Exception
    {
        public IReadOnlyList<string> Errors { get; }

        public BuilderValidationException(IReadOnlyList<string> errors)
            : base(BuildMessage(errors))
        {
            Errors = errors;
        }

        private static string BuildMessage(IReadOnlyList<string> errors)
        {
            if (errors == null || errors.Count == 0)
            {
                return "Builder validation failed.";
            }

            return "Builder validation failed: " + string.Join("; ", errors);
        }
    }
}
