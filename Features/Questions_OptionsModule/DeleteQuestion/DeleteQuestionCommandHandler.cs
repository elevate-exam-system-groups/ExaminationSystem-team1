using ExaminationSystem.Features.Common.Enums;
using ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion.DTOs;

namespace ExaminationSystem.Features.Questions_OptionsModule.DeleteQuestion
{
    public class DeleteQuestionCommandHandler 
        : IRequestHandler<DeleteQuestionCommand, RequestResult<DeleteQuestionResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteQuestionCommandHandler(IUnitOfWork unitOfWork)
            => _unitOfWork = unitOfWork;

        public async Task<RequestResult<DeleteQuestionResponse>> Handle(
            DeleteQuestionCommand request, CancellationToken ct)
        {
            var questionRepo = _unitOfWork.GetRepository<Question>();
            var question = await questionRepo.GetById(request.Id)
                .Include(q => q.Quiz)
                .FirstOrDefaultAsync(ct);

            if (question == null)
                return RequestResult<DeleteQuestionResponse>.Failure("Question not found", RequestErrorCode.UserNotFound);

            if (question.Quiz.Status == QuizStatus.Published)
                return RequestResult<DeleteQuestionResponse>.Failure(
                    "Cannot delete question from published quiz. Unpublish quiz first.",
                    RequestErrorCode.Conflict);

            // Soft delete question
            questionRepo.SoftDelete(question);

            // Soft delete all options
            var optionRepo = _unitOfWork.GetRepository<Option>();
            var options = await optionRepo.Get(o => o.QuestionId == question.Id).ToListAsync(ct);
            foreach (var opt in options)
            {
                optionRepo.SoftDelete(opt);
            }

            await _unitOfWork.SaveChangesAsync();

            return RequestResult<DeleteQuestionResponse>.Success(
                new DeleteQuestionResponse(true),
                "Question soft-deleted successfully");
        }
    }

}