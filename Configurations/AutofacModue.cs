using Autofac;
using AutoMapper;
using ExaminationSystem.Controllers.DiplomaController.ViewModels;
using ExaminationSystem.Controllers.Shared.Middlewares;
using ExaminationSystem.Domain.Implementations;
using ExaminationSystem.Features.Account.Shared.Services;
using ExaminationSystem.Features.AuthModule.Shared;
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

            builder.RegisterGeneric(typeof(HandlerBasicParameterss<>))
                   .AsSelf()
                  .InstancePerLifetimeScope();

            #region register AutoMapper

            builder.Register(context =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<ViewDiplomaQuizzesResponseVMProfile>();

                });
                return config;
            }).SingleInstance().AutoActivate().AsSelf();

            builder.Register(tempContext =>
            {
                var ctx = tempContext.Resolve<IComponentContext>();
                var config = ctx.Resolve<MapperConfiguration>();
                return config.CreateMapper(ctx.Resolve);
            }).As<IMapper>().InstancePerLifetimeScope();

            #endregion

        }
    }
}
