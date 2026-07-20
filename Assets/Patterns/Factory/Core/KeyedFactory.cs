using System;
using System.Collections.Generic;

namespace DesignPatterns.Factory
{
    /// <summary>
    /// Generic factory registry: maps a key to a creator, so client code requests
    /// a product BY KEY without knowing the concrete type or how it's built.
    ///
    /// Adding a product is a <see cref="Register(TKey, Func{TProduct})"/> call, not
    /// a new <c>case</c> in a switch — so the factory is open for extension and
    /// closed for modification. (The legacy shape factory this replaced grew a
    /// switch statement that every new shape had to edit.)
    /// </summary>
    /// <typeparam name="TKey">What selects a creator (enum, char, string, Type…).</typeparam>
    /// <typeparam name="TProduct">The type produced.</typeparam>
    public class KeyedFactory<TKey, TProduct>
    {
        private readonly Dictionary<TKey, Func<TProduct>> _creators;

        public KeyedFactory(IEqualityComparer<TKey> keyComparer = null)
        {
            _creators = new Dictionary<TKey, Func<TProduct>>(keyComparer ?? EqualityComparer<TKey>.Default);
        }

        /// <summary>The keys that currently have a creator.</summary>
        public IReadOnlyCollection<TKey> Keys => _creators.Keys;

        /// <summary>Register a creation delegate for a key. A later registration replaces an earlier one.</summary>
        public KeyedFactory<TKey, TProduct> Register(TKey key, Func<TProduct> creator)
        {
            if (creator == null)
            {
                throw new ArgumentNullException(nameof(creator));
            }

            _creators[key] = creator;
            return this;
        }

        /// <summary>Register a factory object for a key — for creators that need their own state or dependencies.</summary>
        public KeyedFactory<TKey, TProduct> Register(TKey key, IFactory<TProduct> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            return Register(key, factory.Create);
        }

        public bool CanCreate(TKey key) => _creators.ContainsKey(key);

        /// <summary>Create the product for a key, throwing <see cref="UnknownFactoryKeyException"/> if none is registered.</summary>
        public TProduct Create(TKey key)
        {
            if (!_creators.TryGetValue(key, out var creator))
            {
                throw new UnknownFactoryKeyException(key);
            }

            return creator();
        }

        /// <summary>Create the product for a key if one is registered.</summary>
        public bool TryCreate(TKey key, out TProduct product)
        {
            if (_creators.TryGetValue(key, out var creator))
            {
                product = creator();
                return true;
            }

            product = default;
            return false;
        }
    }

    /// <summary>
    /// A keyed factory whose creators take a construction argument (e.g. a spawn
    /// position). Same registry idea as <see cref="KeyedFactory{TKey,TProduct}"/>,
    /// with the argument threaded through to each creator.
    /// </summary>
    /// <typeparam name="TKey">What selects a creator.</typeparam>
    /// <typeparam name="TArg">The argument passed to the chosen creator.</typeparam>
    /// <typeparam name="TProduct">The type produced.</typeparam>
    public class KeyedFactory<TKey, TArg, TProduct>
    {
        private readonly Dictionary<TKey, Func<TArg, TProduct>> _creators;

        public KeyedFactory(IEqualityComparer<TKey> keyComparer = null)
        {
            _creators = new Dictionary<TKey, Func<TArg, TProduct>>(keyComparer ?? EqualityComparer<TKey>.Default);
        }

        public IReadOnlyCollection<TKey> Keys => _creators.Keys;

        public KeyedFactory<TKey, TArg, TProduct> Register(TKey key, Func<TArg, TProduct> creator)
        {
            if (creator == null)
            {
                throw new ArgumentNullException(nameof(creator));
            }

            _creators[key] = creator;
            return this;
        }

        public KeyedFactory<TKey, TArg, TProduct> Register(TKey key, IFactory<TArg, TProduct> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            return Register(key, factory.Create);
        }

        public bool CanCreate(TKey key) => _creators.ContainsKey(key);

        public TProduct Create(TKey key, TArg arg)
        {
            if (!_creators.TryGetValue(key, out var creator))
            {
                throw new UnknownFactoryKeyException(key);
            }

            return creator(arg);
        }

        public bool TryCreate(TKey key, TArg arg, out TProduct product)
        {
            if (_creators.TryGetValue(key, out var creator))
            {
                product = creator(arg);
                return true;
            }

            product = default;
            return false;
        }
    }
}
