namespace DesignPatterns.Decorator.Sample
{
    /// <summary>Adds a flat bonus to whatever it wraps.</summary>
    public sealed class SharpenEnchantment : Decorator<IDamageSource>, IDamageSource
    {
        private readonly int _bonus;

        public SharpenEnchantment(IDamageSource inner, int bonus = 3) : base(inner)
        {
            _bonus = bonus;
        }

        public int GetDamage() => Component.GetDamage() + _bonus;

        public string Describe() => $"{Component.Describe()} +Sharpen({_bonus})";
    }

    /// <summary>Adds flat fire damage on top of whatever it wraps.</summary>
    public sealed class FireEnchantment : Decorator<IDamageSource>, IDamageSource
    {
        private readonly int _fireDamage;

        public FireEnchantment(IDamageSource inner, int fireDamage = 5) : base(inner)
        {
            _fireDamage = fireDamage;
        }

        public int GetDamage() => Component.GetDamage() + _fireDamage;

        public string Describe() => $"{Component.Describe()} +Fire({_fireDamage})";
    }

    /// <summary>
    /// Multiplies the wrapped damage. Because it multiplies rather than adds,
    /// where it sits in the chain changes the total — the clearest demonstration
    /// that decorator order matters.
    /// </summary>
    public sealed class CriticalStrike : Decorator<IDamageSource>, IDamageSource
    {
        private readonly int _multiplier;

        public CriticalStrike(IDamageSource inner, int multiplier = 2) : base(inner)
        {
            _multiplier = multiplier;
        }

        public int GetDamage() => Component.GetDamage() * _multiplier;

        public string Describe() => $"({Component.Describe()}) xCrit({_multiplier})";
    }
}
