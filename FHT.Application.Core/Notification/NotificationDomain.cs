using MediatR;
using System;

namespace FHT.Application.Core.Notification
{
    public class NotificationDomain : INotification
    {
        public NotificationDomain(string message)
        {
            Timestamp = DateTimeOffset.Now;
            Message = message;
        }

        public NotificationDomain(string messageId, string message)
        {
            Timestamp = DateTimeOffset.Now;
            MessageId = messageId;
            Message = message;
        }

        public NotificationDomain(string messageId, string message, int aggregateId)
        {
            Timestamp = DateTimeOffset.Now;
            MessageId = messageId;
            Message = message;
            AggregateId = aggregateId;
        }

        public DateTimeOffset Timestamp { get; private set; }
        public string MessageId { get; private set; }
        public string Message { get; private set; }
        public int AggregateId { get; private set; }
    }
}
