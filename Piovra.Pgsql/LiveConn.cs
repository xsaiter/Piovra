using Npgsql;
using System;
using System.Data;
using System.Threading;

namespace Piovra.Pgsql {
    public class LiveConn : IDisposable {
        NpgsqlConnection _conn;
        readonly Config _cfg;        

        public LiveConn(Config cfg) {
            _cfg = cfg ?? throw new ArgumentNullException(nameof(cfg));
        }

        public LiveConn(string connString) : this(Config.Default(connString)) {
        }

        public NpgsqlConnection Get() {
            if (!OK) {
                CleanUp();
                _conn = New(_cfg);
            }            
            return _conn;
        }

        public bool OK => _conn != null && _conn.State == ConnectionState.Open;

        public void Dispose() => CleanUp();        

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

        public static NpgsqlConnection New(string connString) => New(Config.Default(connString));

        void CleanUp() {
            if (_conn != null) {
                _conn.Dispose();
                _conn = null;
            }
        }

        public class Config {
            public string ConnString { get; set; }
            public int MaxAttempts { get; set; }
            public int TimeBetweenAttemptsInMs { get; set; }

            public const int DEFAULT_TIME_BETWEEN_ATTEMPTS_IN_MS = 5000;
            public const int DEFAULT_MAX_ATTEMPTS = 10;

            public static Config Default(string connString) =>
                new Config {
                    ConnString = connString,
                    MaxAttempts = DEFAULT_MAX_ATTEMPTS,
                    TimeBetweenAttemptsInMs = DEFAULT_TIME_BETWEEN_ATTEMPTS_IN_MS
                };
        }
    }
}