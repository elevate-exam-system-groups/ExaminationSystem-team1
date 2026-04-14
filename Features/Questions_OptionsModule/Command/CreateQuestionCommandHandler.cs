namespace ExaminationSystem.Features.Questions_OptionsModule.Command
{

    #region MyRegion
    public record CreateQuestionCommand : IRequest<RequestResult<CreateQuestionResponse>>
    {
        public Guid QuizId { get; set; }
        public string Text { get; set; }
        public string? Explanation { get; set; }
        public int OrderIndex { get; set; }
    }
    #endregion

    #region Handler
    public class CreateQuestionCommandHandler
   : IRequestHandler<CreateQuestionCommand, RequestResult<CreateQuestionResponse>>
    {

        private readonly IUnitOfWork _unitOfWork;
        public CreateQuestionCommandHandler(IUnitOfWork unitOfWork)
         => _unitOfWork = unitOfWork;


        public async Task<RequestResult<CreateQuestionResponse>> Handle
            (CreateQuestionCommand request, CancellationToken ct)
        {
            var question = new Question
            {
                QuizId = request.QuizId,
                Text = request.Text,
                Explanation = request.Explanation,
                OrderIndex = request.OrderIndex
            };

            var questionRepo = _unitOfWork.GetRepository<Question>();
            questionRepo.Add(question);
            await _unitOfWork.SaveChangesAsync();

            return RequestResult<CreateQuestionResponse>.Success(
                new CreateQuestionResponse(question.Id),
                "Question created successfully");
        }
    } 
    #endregion

}
