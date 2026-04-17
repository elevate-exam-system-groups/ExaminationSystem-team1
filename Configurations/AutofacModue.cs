using Autofac;
using ExaminationSystem.Controllers.Shared.Middlewares;
using ExaminationSystem.Domain.Implementations;
using ExaminationSystem.Features.Account.Shared.Services;
using ExaminationSystem.Features.AuthModule.Shared;
using FluentValidation;
using Module = Autofac.Module;

namespace ExaminationSystem.Configurations
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(GeneralRepository<>))
                   .As(typeof(IGeneralRepository<>))
                   .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
                   .As<IUnitOfWork>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<TokenGenerator>()
                   .As<ITokenGenerator>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<EmailService>()
                   .As<IEmailService>()
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .AsClosedTypesOf(typeof(IValidator<>))
                   .AsImplementedInterfaces();

            builder.RegisterType<GlobalErrorHandlerMiddelware>()
                   .InstancePerLifetimeScope();

        }
    }
}
