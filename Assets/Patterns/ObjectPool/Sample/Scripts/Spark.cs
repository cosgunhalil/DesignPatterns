using System;
using UnityEngine;

namespace DesignPatterns.ObjectPool.Sample
{
    /// <summary>
    /// A short-lived pooled particle. It arcs under gravity for a set lifetime,
    /// then asks to be returned via the callback it was launched with — the pool
    /// reuses the same instance for the next spark. Nothing here knows about the
    /// pool's internals; it only holds a "return me" delegate.
    /// </summary>
    public sealed class Spark : MonoBehaviour
    {
        private static readonly Vector3 Gravity = new(0f, -9.81f, 0f);

        private Vector3 _velocity;
        private float _remainingLife;
        private Action<Spark> _onExpired;

        public void Launch(Vector3 velocity, float lifetime, Action<Spark> onExpired)
        {
            _velocity = velocity;
            _remainingLife = lifetime;
            _onExpired = onExpired;
        }

        private void Update()
        {
            _velocity += Gravity * Time.deltaTime;
            transform.position += _velocity * Time.deltaTime;

            _remainingLife -= Time.deltaTime;
            if (_remainingLife <= 0f)
            {
                var expired = _onExpired;
                _onExpired = null; // guard against a double return
                expired?.Invoke(this);
            }
        }
    }
}
