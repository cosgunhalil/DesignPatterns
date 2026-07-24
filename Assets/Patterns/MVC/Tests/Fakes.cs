namespace DesignPatterns.MVC.Tests
{
    /// <summary>A view that records how often it rendered and the last model it saw.</summary>
    internal sealed class RecordingView<TModel> : IView<TModel>
    {
        public int RenderCount { get; private set; }
        public TModel Last { get; private set; }

        public void Render(TModel model)
        {
            RenderCount++;
            Last = model;
        }
    }

    /// <summary>A minimal observable model, to test the Core glue without the sample.</summary>
    internal sealed class CounterModel : ObservableModel
    {
        public int Value { get; private set; }

        public void Increment()
        {
            Value++;
            NotifyChanged();
        }
    }
}
