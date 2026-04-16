namespace ExaminationSystem.Controllers.Shared
{
    public class EndPointBasicParameters<TRequest>
    {
        public EndPointBasicParameters(IMediator mediator, IValidator<TRequest> validator)
        {
            Mediator = mediator;
            Validator = validator;
        }

        public IMediator Mediator { get; }
        public IValidator<TRequest> Validator { get; }
    }
}
