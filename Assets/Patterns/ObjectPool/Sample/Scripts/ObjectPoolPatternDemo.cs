using UnityEngine;

namespace DesignPatterns.ObjectPool.Sample
{
    /// <summary>
    /// Entry point. Press Play — it emits ~20 sparks/second, each living 1.5s, so
    /// roughly 30 are alive at once. The once-per-second log shows the pool
    /// created only a small, bounded number of objects no matter how many
    /// thousands are "emitted": that bounded CountAll is the whole point of pooling.
    /// </summary>
    public sealed class ObjectPoolPatternDemo : MonoBehaviour
    {
        private const float EmitInterval = 0.05f; // ~20 per second
        private const float SparkLifetime = 1.5f;

        private ObjectPool<Spark> _pool;
        private float _emitTimer;
        private float _statTimer;
        private int _totalEmitted;

        private void Start()
        {
            CreateGround();

            _pool = new ObjectPool<Spark>(
                createFunc: CreateSpark,
                onGet: spark => spark.gameObject.SetActive(true),
                onRelease: spark => spark.gameObject.SetActive(false),
                onDestroy: spark => Destroy(spark.gameObject),
                maxSize: 64);

            _pool.Prewarm(16);

            Debug.Log("<b>Object Pool demo</b> — emitting sparks; watch the pool reuse a bounded set of objects.");
        }

        private void Update()
        {
            _emitTimer += Time.deltaTime;
            while (_emitTimer >= EmitInterval)
            {
                _emitTimer -= EmitInterval;
                Emit();
            }

            _statTimer += Time.deltaTime;
            if (_statTimer >= 1f)
            {
                _statTimer -= 1f;
                Debug.Log($"emitted {_totalEmitted} total | pool created {_pool.CountAll} objects " +
                          $"(active {_pool.CountActive}, idle {_pool.CountInactive})");
            }
        }

        private void Emit()
        {
            var spark = _pool.Get();
            spark.transform.position = transform.position;

            var velocity = new Vector3(
                Random.Range(-2f, 2f),
                Random.Range(5f, 8f),
                Random.Range(-2f, 2f));

            spark.Launch(velocity, SparkLifetime, _pool.Release);
            _totalEmitted++;
        }

        private static Spark CreateSpark()
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = "Spark";
            go.transform.localScale = Vector3.one * 0.3f;
            return go.AddComponent<Spark>();
        }

        private static void CreateGround()
        {
            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.localScale = new Vector3(4f, 1f, 4f);
        }
    }
}
