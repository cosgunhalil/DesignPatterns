namespace DesignPatterns.Decorator.Sample
{
    /// <summary>
    /// The concrete component being decorated: a plain weapon with a fixed base
    /// damage. Enchantments wrap it to change the result; the weapon itself never
    /// changes and knows nothing about them.
    /// </summary>
    public sealed class Weapon : IDamageSource
    {
        private readonly string _name;
        private readonly int _baseDamage;

        public Weapon(string name, int baseDamage)
        {
            _name = name;
            _baseDamage = baseDamage;
        }

        public int GetDamage() => _baseDamage;

        public string Describe() => _name;
    }
}
