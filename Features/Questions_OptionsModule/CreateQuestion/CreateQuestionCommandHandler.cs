namespace ExaminationSystem.Features.Questions_OptionsModule.CreateQuestion
{
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
}
