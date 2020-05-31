using System;
using System.Collections.Generic;

namespace Piovra {
    public class RegistryBase {
        static readonly Lazy<RegistryBase> _lazy = new Lazy<RegistryBase>(() => new RegistryBase());
        static RegistryBase Sole => _lazy.Value;
        protected RegistryBase() { }

        Config _config;
        protected static Config GetConfig() => Sole._config;
        public static void ChangeConfig(Config config) => Sole._config = config;

        public static void CleanUp() => Sole.Clean();
        protected virtual void Clean() { }

        public class Config {
            readonly Dictionary<Type, Func<object>> _map = new Dictionary<Type, Func<object>>();
            Config() { }
            public static Config New() => new Config();

            public Config Add<I, R>() where R : I, new() => Add<I, R>(() => new R());

            public Config Add<I, R>(Func<R> create) where R : I {
                var type = typeof(I);
                if (!_map.ContainsKey(type)) {
                    _map.Add(type, () => create());
                }
                return this;
            }

            public I Get<I>() where I : class {
                var type = typeof(I);
                if (_map.ContainsKey(type)) {
                    return (I)_map[type]();
                }
                throw new Exception($"not found: {type}");
            }
        }
    }
}