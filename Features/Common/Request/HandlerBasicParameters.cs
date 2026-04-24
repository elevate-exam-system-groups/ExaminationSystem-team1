namespace ExaminationSystem.Features.Common.Request
{
    public class HandlerBasicParameterss<TRequest>//,TEntity>
    {
        public HandlerBasicParameterss(IMediator mediator, IValidator<TRequest> validator)
        {
            Mediator = mediator;
            Validator = validator;

        }

        public IMediator Mediator { get; }
        public IValidator<TRequest> Validator { get; }

    }

}
