using ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.DTOs;
using Microsoft.EntityFrameworkCore.Query.Internal;

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
            // 1. Get the question along with its quiz
            var questionRepo = _unitOfWork.GetRepository<Question>();
            var questionInfo = await questionRepo.Get(q => q.Id == request.Id)
                .Select(q => new
                {
                    Question = q,  //=====================
                    QuizStatus = q.Quiz.Status
                }).FirstOrDefaultAsync(ct);

            if (questionInfo == null)
                return RequestResult<UpdateQuestionResponse>.Failure
                    ("Question not found", RequestErrorCode.UserNotFound);

            // Check If Quiz Published/ Unpublished
            if (questionInfo.QuizStatus == QuizStatus.Published)
                return RequestResult<UpdateQuestionResponse>.Failure(
                    "Cannot update question in published quiz. Unpublish quiz first.",
                    RequestErrorCode.Conflict);

            var question = questionInfo.Question;
            // Update Question
            question.Text = request.Text;
            question.Explanation = request.Explanation;
            question.UpdatedAt = DateTime.UtcNow;

            // Update Options
            var optionRepo = _unitOfWork.GetRepository<Option>();

            var existingOptions = await optionRepo
                .Get(o => o.QuestionId == question.Id)
                .ToListAsync(ct);

            var incomingOptionIds = request.Options
                .Where(o => o.Id.HasValue)
                .Select(o => o.Id.Value)
                .ToList();

            // Delete options that are not in the incoming list
            foreach (var opt in existingOptions.Where(o => !incomingOptionIds.Contains(o.Id)))
            {
                optionRepo.SoftDelete(opt);
            }

            // Update existing options and add new ones
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