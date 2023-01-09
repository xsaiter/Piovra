using System;
using System.Threading;
using System.Threading.Tasks;

namespace Piovra.Data;
public interface IUnitOfWork : IDisposable, IAsyncDisposable {
    Task Commit(CancellationToken cancellationToken = default);
}
