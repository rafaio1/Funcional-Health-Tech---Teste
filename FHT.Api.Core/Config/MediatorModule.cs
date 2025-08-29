using Autofac;
using FHT.Application.Core.Behaviors;
using FHT.Application.Core.Notification;
using MediatR;
using System.Reflection;

namespace FHT.Api.Core.Config
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            builder.RegisterType<NotificationDomainHandler>()
               .As<INotificationHandler<NotificationDomain>>()
               .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ValidatorCommandBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}
