using Autofac;
using ExaminationSystem.Features.AuthModule.Shared;
using ExaminationSystem.Infrastructure.Implementations;
using Module = Autofac.Module;

namespace ExaminationSystem.Configurations
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>()
                   .As<IUnitOfWork>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<TokenGenerator>()
                   .As<ITokenGenerator>()
                   .InstancePerLifetimeScope();

        }
    }
}
