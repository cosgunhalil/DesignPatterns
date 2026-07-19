using System.Collections.Generic;

namespace DesignPatterns.Builder.Sample
{
    /// <summary>
    /// Fluent builder for <see cref="Character"/>. It passes ITSELF as the
    /// generic self type, so every <c>With…/Add…</c> method returns
    /// <see cref="CharacterBuilder"/> (not the base) and chains stay strongly
    /// typed. It only implements Validate + BuildCore; the validate-then-build
    /// flow lives in the base.
    /// </summary>
    public sealed class CharacterBuilder : Builder<CharacterBuilder, Character>
    {
        private string _name;
        private int _health = 100;
        private float _speed = 1f;
        private readonly List<string> _abilities = new();
        private readonly List<string> _loot = new();

        public CharacterBuilder SetName(string name)
        {
            _name = name;
            return Self;
        }

        public CharacterBuilder SetHealth(int health)
        {
            _health = health;
            return Self;
        }

        public CharacterBuilder SetSpeed(float speed)
        {
            _speed = speed;
            return Self;
        }

        public CharacterBuilder AddAbility(string ability)
        {
            _abilities.Add(ability);
            return Self;
        }

        public CharacterBuilder AddLoot(string item)
        {
            _loot.Add(item);
            return Self;
        }

        protected override void Validate()
        {
            if (string.IsNullOrWhiteSpace(_name))
            {
                AddError("Name is required.");
            }

            if (_health <= 0)
            {
                AddError("Health must be greater than zero.");
            }

            if (_speed < 0f)
            {
                AddError("Speed cannot be negative.");
            }
        }

        protected override Character BuildCore()
        {
            // Copy the lists so later mutation of this builder can't leak into
            // an already-built (supposedly immutable) Character.
            return new Character
            {
                Name = _name,
                Health = _health,
                Speed = _speed,
                Abilities = _abilities.ToArray(),
                Loot = _loot.ToArray()
            };
        }
    }
}
