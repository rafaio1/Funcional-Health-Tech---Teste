using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FHT.Application.Core.Notification
{
    public class NotificationDomainHandler : INotificationHandler<NotificationDomain>
    {
        private List<NotificationDomain> _notifications;

        public NotificationDomainHandler()
        {
            _notifications = new List<NotificationDomain>();
        }

        public void Dispose()
        {
            _notifications = new List<NotificationDomain>();
        }

        public virtual List<NotificationDomain> GetNotifications()
        {
            return _notifications;
        }

        public Task Handle(NotificationDomain notification, CancellationToken cancellationToken)
        {
            _notifications.Add(notification);
            return Task.CompletedTask;
        }

        public virtual bool HasNotifications()
        {
            return _notifications.Any();
        }
    }
}
