using System.Collections.Generic;

namespace FHT.Api.Config
{
    public interface INotifier
    {
        List<Notification> GetNotifications();
        void Handle(Notification notification);
        bool HasNotification();
    }
}
