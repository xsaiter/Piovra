using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Piovra.Data;

public abstract class UnitOfWork(DbContext context) : IUnitOfWork {
    public DbContext Context { get; } = Requires.CheckNotNull(context, nameof(context));

    public Task<int> Commit(CancellationToken cancellationToken = default) {
        return Context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose() {
        Context.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync() {
        await Context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
