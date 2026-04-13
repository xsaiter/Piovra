using Microsoft.EntityFrameworkCore;

namespace Piovra.Data;

public abstract class UnitOfWork(DbContext context) : IUnitOfWork {
    public DbContext Context { get; } = Requires.NotNull(context, nameof(context));

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
