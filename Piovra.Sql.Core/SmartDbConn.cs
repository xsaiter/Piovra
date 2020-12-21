using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Piovra.Sql.Core {
    public class SmartDbConn<C> : IDisposable where C : DbConnection, new() {        
        readonly Config _cfg;
        C _conn;

        public SmartDbConn(Config cfg) => _cfg = ARG.NotNull(cfg, nameof(cfg));

        public SmartDbConn(string connString) : this(new Config(connString)) { }

        public async Task<C> Get() {
            if (!OK) {
                CleanUp();
                _conn = await New(_cfg);
            }
            return _conn;
        }

        public bool OK => _conn?.State == ConnectionState.Open;

        public static async Task<C> New(Config cfg) {
            var attempts = 0;
            var isOpened = false;
            C conn = null;

            while (!isOpened) {
                try {
                    conn = cfg.CreateConn();
                    conn.ConnectionString = cfg.ConnString;
                    await conn.OpenAsync();
                    isOpened = true;
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

        public void Dispose() {
            CleanUp();
        }

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