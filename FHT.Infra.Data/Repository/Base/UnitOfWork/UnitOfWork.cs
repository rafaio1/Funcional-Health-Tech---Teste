using FHT.Infra.Data.Context;
using FHT.Infra.Data.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Infra.Data.Repository.Base.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;
        private readonly IMediator _mediator;
        private readonly ILogger<UnitOfWork> _logger;
        private IDbContextTransaction? _currentTransaction;

        public UnitOfWork(AppDbContext context, IMediator mediator, ILogger<UnitOfWork> logger)
        {
            _context = context;
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Commit simples (sem transação explícita).
        /// </summary>
        public void Commit()
        {
            _context.SaveChanges();
        }

        /// <summary>
        /// Commit simples assíncrono (sem transação explícita).
        /// </summary>
        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _mediator.DispatchDomainEventsAsync(_context, _logger);
            var result = await _context.SaveChangesAsync(cancellationToken);
            _logger.LogTrace("CommitAsync executado com {Result} alterações.", result);
        }

        public void Dispose()
        {
            _currentTransaction?.Dispose();
            _context.Dispose();
        }
    }
}
