using ExaminationSystem.Features.Questions_OptionsModule.CreateQuestion;
using ExaminationSystem.Features.Questions_OptionsModule.CreateQuestion.DTOs;

namespace ExaminationSystem.Features.Questions_OptionsModule.Command
{
    public class CreateQuestionCommandHandler
        : IRequestHandler<CreateQuestionCommand, RequestResult<CreateQuestionResponse>>
    {

        private readonly IUnitOfWork _unitOfWork;
        public CreateQuestionCommandHandler(IUnitOfWork unitOfWork)
            => _unitOfWork = unitOfWork;

        public async Task<RequestResult<CreateQuestionResponse>> Handle(
            CreateQuestionCommand request,
            CancellationToken ct)
        {

            using var transaction = await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                // 1. Check If Quiz Exists and get its status
                var quizRepo = _unitOfWork.GetRepository<Quiz>();
                var quiz = await quizRepo.GetById(request.QuizId).FirstOrDefaultAsync(ct);

                if (quiz == null)
                    return RequestResult<CreateQuestionResponse>.Failure("Quiz not found", RequestErrorCode.UserNotFound);

                if (quiz.Status == QuizStatus.Published)
                    return RequestResult<CreateQuestionResponse>.Failure(
                        "Cannot add question to published quiz",
                        RequestErrorCode.Conflict);

                // 2. Calculate OrderIndex
                var questionRepo = _unitOfWork.GetRepository<Question>();
                var maxOrder = await questionRepo
                    .Get(q => q.QuizId == request.QuizId)
                    .MaxAsync(q => (int?)q.OrderIndex, ct) ?? 0;

                // 3. Create Question
                var question = new Question
                {
                    QuizId = request.QuizId,
                    Text = request.Text,
                    Explanation = request.Explanation,
                    OrderIndex = maxOrder + 1
                };

                questionRepo.Add(question);
                await _unitOfWork.SaveChangesAsync();

                // 4. Create Options
                var optionRepo = _unitOfWork.GetRepository<Option>();
                foreach (var opt in request.Options)
                {
                    optionRepo.Add(new Option
                    {
                        QuestionId = question.Id,
                        Text = opt.Text,
                        IsCorrect = opt.IsCorrect
                    });
                }

                await _unitOfWork.SaveChangesAsync();

                return RequestResult<CreateQuestionResponse>.Success(
                    new CreateQuestionResponse(question.Id),
                    "Question created successfully");

            }
            catch (Exception)
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }
    }
}