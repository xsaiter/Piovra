using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Piovra.Data {
    public interface IUnitOfWork : IDisposable {
        Task Commit(CancellationToken cancellationToken = default);
    }

    public abstract class UnitOfWork : IUnitOfWork {
        protected UnitOfWork(DbContext context) {
            Context = context;
        }

        public DbContext Context { get; }

        public async Task Commit(CancellationToken cancellationToken = default) {
            await Context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose() {
            Context.Dispose();
        }
    }
}
