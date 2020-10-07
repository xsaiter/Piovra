using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Piovra.Data {

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
