using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Piovra.Sql.Core {
    public class SmartConn<C> : IDisposable where C : DbConnection, new() {
        C _conn;
        readonly Config _cfg;

        public SmartConn(Config cfg) => _cfg = cfg ?? throw new ArgumentNullException(nameof(cfg));        
        
        public SmartConn(string connString) : this(new Config(connString)) { }

        public async Task<C> Get() {
            if (!OK) {
                CleanUp();
                _conn = await New(_cfg);
            }
            return _conn;
        }

        public bool OK => _conn != null && _conn.State == ConnectionState.Open;

        public static async Task<C> New(Config cfg) {
            var attempts = 0;
            var opened = false;
            C conn = null;

            while (!opened) {
                try {
                    conn = cfg.CreateConn();
                    conn.ConnectionString = cfg.ConnString;
                    await conn.OpenAsync();
                    opened = true;
                } catch (Exception e) {
                    if (conn != null) {
                        conn.Dispose();
                        conn = null;
                    }
                    if (attempts > cfg.MaxAttempts) {
                        throw new Exception($"max attempts = {cfg.MaxAttempts} is exceeded", e);
                    }
                    Thread.Sleep(cfg.TimeBetweenAttemptsInMs);
                    ++attempts;
                }
            }

            return conn;
        }

        public static Task<C> New(string connString) => New(new Config(connString));

        public void Dispose() => CleanUp();

        void CleanUp() {
            if (_conn != null) {
                _conn.Dispose();
                _conn = null;
            }
        }

        public class Config {
            public const int DEFAULT_TIME_BETWEEN_ATTEMPTS_IN_MS = 5000;
            public const int DEFAULT_MAX_ATTEMPTS = 10;
            public Config(string connString) => ConnString = connString;
            public Func<C> CreateConn { get; set; } = () => new C();            
            public string ConnString { get; set; }
            public int MaxAttempts { get; set; } = DEFAULT_MAX_ATTEMPTS;
            public int TimeBetweenAttemptsInMs { get; set; } = DEFAULT_TIME_BETWEEN_ATTEMPTS_IN_MS;
        }
    }
}
