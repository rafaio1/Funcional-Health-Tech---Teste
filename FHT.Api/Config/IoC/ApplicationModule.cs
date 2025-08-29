using Autofac;
using Autofac.Core; 
using FHT.Infra.Data.Core.Interfaces;
using FHT.Infra.Data.Repository.Base.UnitOfWork;
using MediatR;
using FluentValidation;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using FHT.Application.Read.Command.Cliente;
using FHT.Application.Read.Handler.Cliente;
using FHT.Application.Core.Behaviors;
using FHT.Api.Config;
using System.Collections.Generic;
using System;

namespace FHT.Api.Config.IoC
{
    public class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>()
                   .As<IUnitOfWork>()
                   .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(Repository<>))
                   .AsSelf() 
                   .InstancePerLifetimeScope();

            var infraDataAsm = typeof(UnitOfWork).Assembly; 
            builder.RegisterAssemblyTypes(infraDataAsm)
                   .Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.RegisterType<Mediator>()
                   .As<IMediator>()
                   .InstancePerLifetimeScope();

            builder.Register<SingleInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.TryResolve(t, out var o) ? o : null;
            });
            builder.Register<MultiInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            });

            // Assemblies do App.Read com Commands/Handlers
            var appReadCommandsAsm = typeof(CriarClienteCommand).Assembly;
            var appReadHandlersAsm = typeof(CriarClienteHandler).Assembly;

            builder.RegisterAssemblyTypes(appReadCommandsAsm, appReadHandlersAsm)
                   .AsClosedTypesOf(typeof(IRequestHandler<,>))
                   .AsImplementedInterfaces()
                   .InstancePerDependency();

            builder.RegisterGeneric(typeof(LoggingBehavior<,>))
                   .As(typeof(IPipelineBehavior<,>))
                   .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ValidatorCommandBehavior<,>))
                   .As(typeof(IPipelineBehavior<,>))
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(appReadCommandsAsm)
                   .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                   .AsImplementedInterfaces()
                   .InstancePerDependency();

            builder.RegisterType<HttpContextAccessor>()
                   .As<IHttpContextAccessor>()
                   .SingleInstance();

            builder.RegisterType<Notifier>()
                   .As<INotifier>()
                   .SingleInstance();
        }

        public delegate object? SingleInstanceFactory(Type serviceType);
        public delegate IEnumerable<object> MultiInstanceFactory(Type serviceType);
    }
}
