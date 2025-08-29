using FHT.Domain.Core;
using FHT.Infra.Data.Context;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace FHT.Infra.Data.Repository.Base.UnitOfWork
{
    public static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, AppDbContext ctx, ILogger<UnitOfWork> logger)
        {
            System.Collections.Generic.IEnumerable<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<EntityBase>> domainEntities = ctx.ChangeTracker
                .Entries<EntityBase>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            logger.LogTrace("DispatchDomainEventsAsync Domain Entities", domainEntities);

            System.Collections.Generic.List<INotification> domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            logger.LogTrace("DispatchDomainEventsAsync Domain Events", domainEvents);

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            System.Collections.Generic.IEnumerable<Task> tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    logger.LogTrace("DispatchDomainEventsAsync Publish Domain Events", domainEvent);
                    await mediator.Publish(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
