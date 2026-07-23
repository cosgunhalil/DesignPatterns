namespace DesignPatterns.Decorator.Sample
{
    /// <summary>
    /// The component interface. A base weapon implements it; each enchantment
    /// both implements it and wraps another <see cref="IDamageSource"/>, so
    /// enchantments stack in any combination without a subclass per combination.
    /// </summary>
    public interface IDamageSource
    {
        /// <summary>The final damage after every wrapping enchantment has had its say.</summary>
        int GetDamage();

        /// <summary>A human-readable view of the composition, e.g. "(Sword +Sharpen) xCrit".</summary>
        string Describe();
    }
}
