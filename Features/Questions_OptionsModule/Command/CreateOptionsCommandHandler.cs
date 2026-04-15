namespace ExaminationSystem.Features.Questions_OptionsModule.CreateOptions
{

    #region Request
    public record CreateOptionsCommand(Guid QuestionId, List<OptionDto> Options)
     : IRequest<RequestResult<CreateOptionsResponse>>;

    #endregion

    #region Handler
    public class CreateOptionsCommandHandler
   : IRequestHandler<CreateOptionsCommand, RequestResult<CreateOptionsResponse>>
    {

        private readonly IUnitOfWork _unitOfWork;
        public CreateOptionsCommandHandler(IUnitOfWork unitOfWork)
         => _unitOfWork = unitOfWork;


        public async Task<RequestResult<CreateOptionsResponse>> Handle
            (CreateOptionsCommand request, CancellationToken ct)
        {
            var optionRepo = _unitOfWork.GetRepository<Option>();

            var options = request.Options.Select(opt => new Option
            {
                QuestionId = request.QuestionId,
                Text = opt.Text,
                IsCorrect = opt.IsCorrect
            }).ToList();

            optionRepo.AddRange(options);
            await _unitOfWork.SaveChangesAsync();

            var response = new CreateOptionsResponse(
                options.Select(o => o.Id).ToList(),
                options.Count
            );

            return RequestResult<CreateOptionsResponse>.Success(response, "Options created successfully");
        }
    } 
    #endregion

}
