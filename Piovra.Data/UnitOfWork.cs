using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Piovra.Data {
    public interface IUnitOfWork : IDisposable, IAsyncDisposable {
        Task Commit(CancellationToken cancellationToken = default);
    }

    public abstract class UnitOfWork : IUnitOfWork {
        protected UnitOfWork(DbContext context) => Context = ARG.NotNull(context, nameof(context));        

        public DbContext Context { get; }

        public Task Commit(CancellationToken cancellationToken = default) {
            return Context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose() {
            Context.Dispose();
        }

        public ValueTask DisposeAsync() {
            return Context.DisposeAsync();
        }
    }
}
