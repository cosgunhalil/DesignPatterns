using System;

namespace DesignPatterns.MVC
{
    /// <summary>
    /// Base for controllers. A controller turns user intent into model changes —
    /// it holds the model and exposes intent methods (Increase, Toggle, Submit…)
    /// that mutate it. It deliberately does NOT render: the view refreshes itself
    /// in response to the model's <see cref="ObservableModel.Changed"/> event, so
    /// controller and view never touch each other.
    /// </summary>
    /// <typeparam name="TModel">The model this controller drives.</typeparam>
    public abstract class Controller<TModel> where TModel : ObservableModel
    {
        protected TModel Model { get; }

        protected Controller(TModel model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }
    }
}
