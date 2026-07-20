namespace DesignPatterns.Factory.Tests
{
    /// <summary>Object-based creator, for testing the IFactory register overload.</summary>
    internal sealed class ConstantFactory : IFactory<string>
    {
        private readonly string _value;

        public ConstantFactory(string value)
        {
            _value = value;
        }

        public string Create() => _value;
    }

    /// <summary>Parameterized object-based creator: repeats a char N times.</summary>
    internal sealed class RepeatFactory : IFactory<int, string>
    {
        private readonly char _character;

        public RepeatFactory(char character)
        {
            _character = character;
        }

        public string Create(int count) => new string(_character, count);
    }
}
