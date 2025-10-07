using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Piovra.Sql.Core;

public class SmartDbConn<C>(SmartDbConn<C>.Config cfg)
    : IDisposable where C : DbConnection, new() {

    readonly Config _cfg = Requires.CheckNotNull(cfg, nameof(cfg));
    C _conn;

    public SmartDbConn(string connString) : this(new Config(connString)) { }

    public async Task<C> GetAsync(CancellationToken cancellationToken = default) {
        if (!IsSuccess()) {
            CleanUp();
            _conn = await NewAsync(_cfg, cancellationToken);
        }
        return _conn;
    }

    public bool IsSuccess() => _conn?.State == ConnectionState.Open;

    public static async Task<C> NewAsync(Config cfg, CancellationToken cancellationToken = default) {
        var attempts = 0;
        var isOpened = false;
        C conn = null;

        while (!isOpened) {
            try {
                conn = cfg.CreateConn();
                conn.ConnectionString = cfg.ConnString;
                await conn.OpenAsync(cancellationToken);
                isOpened = true;
            } catch (Exception e) {
                if (conn != null) {
                    await conn.DisposeAsync();
                    conn = null;
                }
                if (attempts > cfg.MaxAttempts) {
                    throw new Exception($"Max attempts = {cfg.MaxAttempts} is exceeded", e);
                }
                await Task.Delay(cfg.TimeBetweenAttemptsInMs, cancellationToken);
                ++attempts;
            }
        }

        return conn;
    }

    public static Task<C> NewAsync(string connString, CancellationToken cancellationToken = default)
        => NewAsync(new Config(connString), cancellationToken);

    public void Dispose() {
        CleanUp();
        GC.SuppressFinalize(this);
    }

    void CleanUp() {
        _conn?.Dispose();
        _conn = null;
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
