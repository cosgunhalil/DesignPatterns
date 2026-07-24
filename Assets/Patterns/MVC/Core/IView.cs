namespace DesignPatterns.MVC
{
    /// <summary>
    /// Renders a model. A view is passive: it reads model state and displays it,
    /// and never mutates the model — user actions go to the controller, not here.
    /// Contravariant so a view of a base model type can render a derived one.
    /// </summary>
    /// <typeparam name="TModel">The model type this view displays.</typeparam>
    public interface IView<in TModel>
    {
        void Render(TModel model);
    }
}
