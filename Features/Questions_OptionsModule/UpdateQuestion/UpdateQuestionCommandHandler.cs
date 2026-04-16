using ExaminationSystem.Features.Common.Enums;
using ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.DTOs;

namespace ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion
{

    public class UpdateQuestionCommandHandler 
        : IRequestHandler<UpdateQuestionCommand, RequestResult<UpdateQuestionResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateQuestionCommandHandler(IUnitOfWork unitOfWork)
            => _unitOfWork = unitOfWork;

        public async Task<RequestResult<UpdateQuestionResponse>> Handle(
            UpdateQuestionCommand request, CancellationToken ct)
        {
            var questionRepo = _unitOfWork.GetRepository<Question>();
            var question = await questionRepo.GetById(request.Id)
                .Include(q => q.Quiz)
                .FirstOrDefaultAsync(ct);

            if (question == null)
                return RequestResult<UpdateQuestionResponse>.Failure("Question not found", RequestErrorCode.UserNotFound);

            // التحقق من حالة الـ Quiz
            if (question.Quiz.Status == QuizStatus.Published)
                return RequestResult<UpdateQuestionResponse>.Failure(
                    "Cannot update question in published quiz. Unpublish quiz first.",
                    RequestErrorCode.Conflict);

            // تحديث الـ Question
            question.Text = request.Text;
            question.Explanation = request.Explanation;
            question.UpdatedAt = DateTime.UtcNow;

            // تحديث الـ Options
            var optionRepo = _unitOfWork.GetRepository<Option>();
            var existingOptions = await optionRepo
                .Get(o => o.QuestionId == question.Id)
                .ToListAsync(ct);

            var incomingOptionIds = request.Options
                .Where(o => o.Id.HasValue)
                .Select(o => o.Id.Value)
                .ToList();

            // حذف اللي مش موجودين
            foreach (var opt in existingOptions.Where(o => !incomingOptionIds.Contains(o.Id)))
            {
                optionRepo.SoftDelete(opt);
            }

            // تحديث أو إضافة
            foreach (var opt in request.Options)
            {
                if (opt.Id.HasValue)
                {
                    var existing = existingOptions.FirstOrDefault(o => o.Id == opt.Id.Value);
                    if (existing != null)
                    {
                        existing.Text = opt.Text;
                        existing.IsCorrect = opt.IsCorrect;
                        existing.UpdatedAt = DateTime.UtcNow;
                    }
                }
                else
                {
                    optionRepo.Add(new Option
                    {
                        QuestionId = question.Id,
                        Text = opt.Text,
                        IsCorrect = opt.IsCorrect
                    });
                }
            }

            await _unitOfWork.SaveChangesAsync();

            return RequestResult<UpdateQuestionResponse>.Success(
                new UpdateQuestionResponse(question.Id),
                "Question updated successfully");
        }
    }
}