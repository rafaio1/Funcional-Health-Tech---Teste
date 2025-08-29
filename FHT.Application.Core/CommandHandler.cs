using FHT.Application.Core.Notification;
using FHT.Domain.Core;
using FHT.Infra.Data.Authorization;
using FHT.Infra.Data.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FHT.Application.Core
{
    public abstract class CommandHandler
    {
        protected readonly IUnitOfWork _uow;
        protected readonly IMediator _mediator;
        protected readonly INotificationHandler<NotificationDomain> _notifications;
        protected readonly ILogger<CommandHandler> _logger;
        protected readonly IAspNetUser _user;

        public CommandHandler()
        {

        }

        public CommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public CommandHandler(IMediator mediator, INotificationHandler<NotificationDomain> notifications)
        {
            _mediator = mediator;
            _notifications = notifications;
        }

        public CommandHandler(IUnitOfWork uow, IMediator mediator, INotificationHandler<NotificationDomain> notifications)
        {
            _uow = uow;
            _mediator = mediator;
            _notifications = notifications;
        }

        public CommandHandler(IUnitOfWork uow, IMediator mediator, INotificationHandler<NotificationDomain> notifications, ILogger<CommandHandler> logger = null)
        {
            _uow = uow;
            _mediator = mediator;
            _notifications = notifications;
            _logger = logger;
        }

        public CommandHandler(IUnitOfWork uow, IMediator mediator, INotificationHandler<NotificationDomain> notifications, IAspNetUser user = null, ILogger<CommandHandler> logger = null)
        {
            _uow = uow;
            _mediator = mediator;
            _notifications = notifications;
            _user = user;
            _logger = logger;
        }

        protected async void HandleEntity(EntityBase entity)
        {
            if (entity == null)
            {
                return;
            }

            foreach (FluentValidation.Results.ValidationFailure error in entity.ValidationResult.Errors)
            {
                await _mediator.Publish(new NotificationDomain(error.PropertyName, error.ErrorMessage));
            }
        }

        protected async void HandleEntities(IEnumerable<EntityBase> entities)
        {
            if (entities == null)
            {
                return;
            }

            foreach (EntityBase item in entities)
            {
                foreach (FluentValidation.Results.ValidationFailure error in item.ValidationResult.Errors)
                {
                    await _mediator.Publish(new NotificationDomain(error.PropertyName, error.ErrorMessage));
                }
            }
        }

        protected async void AddNotification(string key, string message)
        {
            await _mediator.Publish(new NotificationDomain(key, message));
        }

        public async void AddNotification(NotificationDomain notification)
        {
            await _mediator.Publish(new NotificationDomain(notification.MessageId, notification.Message));
        }

        public async void AddNotifications(IReadOnlyCollection<NotificationDomain> notifications)
        {
            foreach (NotificationDomain item in notifications)
            {
                await _mediator.Publish(new NotificationDomain(item.MessageId, item.Message));
            }
        }

        public async void AddNotifications(IList<NotificationDomain> notifications)
        {
            foreach (NotificationDomain item in notifications)
            {
                await _mediator.Publish(new NotificationDomain(item.MessageId, item.Message));
            }
        }

        public async void AddNotifications(ICollection<NotificationDomain> notifications)
        {
            foreach (NotificationDomain item in notifications)
            {
                await _mediator.Publish(new NotificationDomain(item.MessageId, item.Message));
            }
        }

        protected bool IsSuccess()
        {
            return !((NotificationDomainHandler)_notifications).HasNotifications();
        }

        protected List<NotificationDomain> DomainNotifications()
        {
            return ((NotificationDomainHandler)_notifications).GetNotifications();
        }

        protected async Task CommitAsync()
        {
            _logger.LogTrace("Is Success Domain Notificações", IsSuccess());

            if (!IsSuccess())
            {
                return;
            }

            await _uow.CommitAsync();

            _logger.LogTrace("Is Commit DataBase", true);
        }
    }
}
