namespace DesignPatterns.Component
{
    /// <summary>
    /// Implemented by components that need a per-frame tick. <see cref="Entity.Update"/>
    /// calls <see cref="Update"/> on every component that has this capability and
    /// skips the ones that don't — so a purely passive component (say, one that
    /// just holds state) costs nothing per frame.
    /// </summary>
    public interface IUpdatable
    {
        void Update(float deltaTime);
    }
}
