using UnityEngine;

namespace DesignPatterns.Command.Sample
{
    /// <summary>
    /// The receiver: it knows HOW to act, commands decide WHEN.
    /// Note it has no idea commands, input, or undo exist.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Move(Vector3 delta)
        {
            transform.position += delta;
        }

        public void Jump(float force)
        {
            _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
        }
    }
}
