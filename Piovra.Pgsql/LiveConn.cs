using System;
using System.Data;
using System.Threading;
using Npgsql;

namespace Piovra.Pgsql {
    public class LiveConn : IDisposable {
        NpgsqlConnection _conn;
        readonly Config _cfg;

        public LiveConn(Config cfg) {
            _cfg = cfg ?? throw new ArgumentNullException(nameof(cfg));
        }

        public LiveConn(string connString) : this(new Config { ConnString = connString }) { }

        public NpgsqlConnection Get() {
            if (!OK) {
                CleanUp();
                _conn = New(_cfg);
            }
            return _conn;
        }

        public bool OK => _conn != null && _conn.State == ConnectionState.Open;        

        public static NpgsqlConnection New(Config cfg) {
            var attempts = 0;
            var opened = false;
            NpgsqlConnection conn = null;

            while (!opened) {
                try {
                    conn = new NpgsqlConnection(cfg.ConnString);
                    conn.Open();
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

        public static NpgsqlConnection New(string connString) => New(new Config { ConnString = connString });

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
            public string ConnString { get; set; }
            public int MaxAttempts { get; set; } = DEFAULT_MAX_ATTEMPTS;
            public int TimeBetweenAttemptsInMs { get; set; } = DEFAULT_TIME_BETWEEN_ATTEMPTS_IN_MS;
        }
    }
}