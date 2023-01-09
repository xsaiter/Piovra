using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Piovra.Data;

public abstract class UnitOfWork : IUnitOfWork {
    protected UnitOfWork(DbContext context) => Context = ARG.NotNull(context, nameof(context));

    public DbContext Context { get; }

    public Task Commit(CancellationToken cancellationToken = default) {
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
