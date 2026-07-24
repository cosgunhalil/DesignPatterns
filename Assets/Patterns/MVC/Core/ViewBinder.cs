using System;

namespace DesignPatterns.MVC
{
    /// <summary>
    /// The glue that keeps a view in sync with a model: it renders once on
    /// creation, then re-renders on every <see cref="ObservableModel.Changed"/>.
    /// Keeping this wiring here lets the view stay a pure render function and the
    /// model stay unaware of views. Dispose to unbind — important in Unity, where
    /// a view outliving its model (or vice versa across scene loads) leaks.
    /// </summary>
    /// <typeparam name="TModel">The observable model type.</typeparam>
    public sealed class ViewBinder<TModel> : IDisposable where TModel : ObservableModel
    {
        private readonly TModel _model;
        private readonly IView<TModel> _view;
        private bool _bound;

        public ViewBinder(TModel model, IView<TModel> view)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _view = view ?? throw new ArgumentNullException(nameof(view));

            _model.Changed += RenderModel;
            _bound = true;

            RenderModel(); // show the initial state immediately
        }

        private void RenderModel() => _view.Render(_model);

        public void Dispose()
        {
            if (!_bound)
            {
                return;
            }

            _model.Changed -= RenderModel;
            _bound = false;
        }
    }
}
