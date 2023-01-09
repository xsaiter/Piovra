using System;
using System.Threading;
using System.Threading.Tasks;

namespace Piovra.Data;

public interface IUnitOfWork : IDisposable, IAsyncDisposable {
    Task<int> Commit(CancellationToken cancellationToken = default);
}
