using ExaminationSystem.Features.Questions_OptionsModule.CreateQuestion;
using ExaminationSystem.Features.Questions_OptionsModule.CreateQuestion.DTOs;

namespace ExaminationSystem.Features.Questions_OptionsModule.Command
{
    public class AddQuestionCommandHandler
        : IRequestHandler<AddQuestionCommand, RequestResult<AddQuestionResponse>>
    {

        private readonly IUnitOfWork _unitOfWork;
        public AddQuestionCommandHandler(IUnitOfWork unitOfWork)
            => _unitOfWork = unitOfWork;

        public async Task<RequestResult<AddQuestionResponse>> Handle(
            AddQuestionCommand request,
            CancellationToken ct)
        {

            using var transaction = await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                // 1. Check If Quiz Exists and get its status
                var quizRepo = _unitOfWork.GetRepository<Quiz>();
                var quiz = await quizRepo.GetById(request.QuizId)  //===================
                                         .FirstOrDefaultAsync(ct);

                if (quiz == null)
                    return RequestResult<AddQuestionResponse>.Failure
                        ("Quiz not found", RequestErrorCode.UserNotFound);

                if (quiz.Status == QuizStatus.Published)
                    return RequestResult<AddQuestionResponse>.Failure(
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

                return RequestResult<AddQuestionResponse>.Success(
                    new AddQuestionResponse(question.Id),
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