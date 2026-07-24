using System;
using NUnit.Framework;

namespace DesignPatterns.MVC.Tests
{
    public class ViewBinderTests
    {
        [Test]
        public void Binding_RendersTheInitialStateImmediately()
        {
            var model = new CounterModel();
            var view = new RecordingView<CounterModel>();

            using var binder = new ViewBinder<CounterModel>(model, view);

            Assert.AreEqual(1, view.RenderCount);
            Assert.AreSame(model, view.Last);
        }

        [Test]
        public void ModelChange_ReRendersTheView()
        {
            var model = new CounterModel();
            var view = new RecordingView<CounterModel>();
            using var binder = new ViewBinder<CounterModel>(model, view);

            model.Increment();
            model.Increment();

            Assert.AreEqual(3, view.RenderCount); // 1 initial + 2 changes
        }

        [Test]
        public void Dispose_StopsFurtherRenders()
        {
            var model = new CounterModel();
            var view = new RecordingView<CounterModel>();
            var binder = new ViewBinder<CounterModel>(model, view);

            binder.Dispose();
            model.Increment();

            Assert.AreEqual(1, view.RenderCount); // only the initial render
        }

        [Test]
        public void Dispose_Twice_IsHarmless()
        {
            var binder = new ViewBinder<CounterModel>(new CounterModel(), new RecordingView<CounterModel>());
            binder.Dispose();

            Assert.DoesNotThrow(() => binder.Dispose());
        }

        [Test]
        public void Constructor_NullArguments_Throw()
        {
            Assert.Throws<ArgumentNullException>(() => new ViewBinder<CounterModel>(null, new RecordingView<CounterModel>()));
            Assert.Throws<ArgumentNullException>(() => new ViewBinder<CounterModel>(new CounterModel(), null));
        }
    }
}
