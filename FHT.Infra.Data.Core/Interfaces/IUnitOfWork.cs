using System;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Infra.Data.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Commit simples sem transação explícita.
        /// </summary>
        void Commit();

        /// <summary>
        /// Commit simples assíncrono sem transação explícita.
        /// </summary>
        Task CommitAsync(CancellationToken cancellationToken = default);
    }
}
