using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Piovra.Sql.Core;

public class SmartDbConn<C>(SmartDbConn<C>.Config cfg) : IDisposable where C : DbConnection, new() {
    readonly Config _cfg = ARG.CheckNotNull(cfg, nameof(cfg));
    C _conn;

    public SmartDbConn(string connString) : this(new Config(connString)) { }

    public async Task<C> GetAsync() {
        if (!OK) {
            CleanUp();
            _conn = await NewAsync(_cfg);
        }
        return _conn;
    }

    public bool OK => _conn?.State == ConnectionState.Open;

    public static async Task<C> NewAsync(Config cfg) {
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
                    throw new Exception($"Max attempts = {cfg.MaxAttempts} is exceeded", e);
                }
                await Task.Delay(cfg.TimeBetweenAttemptsInMs);
                ++attempts;
            }
        }

        return conn;
    }

    public static Task<C> NewAsync(string connString) => NewAsync(new Config(connString));

    public void Dispose() {
        CleanUp();
        GC.SuppressFinalize(this);
    }

    void CleanUp() {
        if (_conn is not null) {
            _conn.Dispose();
            _conn = null;
        }
    }

    public class Config(string connString) {
        public const int DEFAULT_TIME_BETWEEN_ATTEMPTS_IN_MS = 5000;
        public const int DEFAULT_MAX_ATTEMPTS = 10;

        public Func<C> CreateConn { get; set; } = () => new C();
        public string ConnString { get; set; } = connString;
        public int MaxAttempts { get; set; } = DEFAULT_MAX_ATTEMPTS;
        public int TimeBetweenAttemptsInMs { get; set; } = DEFAULT_TIME_BETWEEN_ATTEMPTS_IN_MS;
    }
}
