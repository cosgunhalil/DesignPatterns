using System;

namespace DesignPatterns.MVC
{
    /// <summary>
    /// Base for MVC models. A model holds state and the rules that keep it valid,
    /// and raises <see cref="Changed"/> whenever that state actually changes — so
    /// views refresh without the model knowing any view exists. The model never
    /// references the view or controller; notification flows one way, outward.
    /// </summary>
    public abstract class ObservableModel
    {
        /// <summary>Raised after the model's state changes.</summary>
        public event Action Changed;

        /// <summary>Subclasses call this from a mutator once a real change has happened.</summary>
        protected void NotifyChanged() => Changed?.Invoke();
    }
}
