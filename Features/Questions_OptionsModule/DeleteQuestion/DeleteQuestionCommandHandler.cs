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
            var questionInfo = await questionRepo.Get(q => q.Id == request.Id)
                .Select(q => new
                {
                    Question = q,  //=====================
                    QuizStatus = q.Quiz.Status
                }).FirstOrDefaultAsync(ct);

            if (questionInfo == null)
                return RequestResult<DeleteQuestionResponse>.Failure
                    ("Question not found", RequestErrorCode.NotFound);

            if (questionInfo.QuizStatus == QuizStatus.Published)
                return RequestResult<DeleteQuestionResponse>.Failure(
                    "Cannot delete question from published quiz. Unpublish quiz first.",
                    RequestErrorCode.Conflict);

            var question = questionInfo.Question;
            // Soft delete question
            questionRepo.SoftDelete(question);

            // Soft delete all options
            var optionRepo = _unitOfWork.GetRepository<Option>();
            var options = await optionRepo.Get(o => o.QuestionId == question.Id)
                                          .ToListAsync(ct);
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