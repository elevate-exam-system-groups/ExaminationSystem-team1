using ExaminationSystem.Features.Questions_OptionsModule.DTOs;
using ExaminationSystem.Features.Questions_OptionsModule.GetNextOrderIndex;

namespace ExaminationSystem.Features.Questions_OptionsModule.AddQuestion
{

    public class AddQuestionCommandHandler : IRequestHandler<AddQuestionCommand, RequestResult<AddQuestionResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public AddQuestionCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<RequestResult<AddQuestionResponse>> Handle(AddQuestionCommand request, CancellationToken cancellationToken)
        {
            // 1. Check if Quiz exists and is not deleted
            var quizRepo = _unitOfWork.GetRepository<Quiz>();
            var quiz = await quizRepo.GetById(request.QuizId)
                                     .FirstOrDefaultAsync(cancellationToken);
            if (quiz == null)
                return RequestResult<AddQuestionResponse>.Failure("Quiz not found");


            if (quiz.Status == QuizStatus.Published)
                return RequestResult<AddQuestionResponse>.Failure(
                    "Cannot add question to a published quiz. Unpublish the quiz first.",
                    RequestErrorCode.ValidationError);

            // 2. Get next order index using MediatR Query
            var orderResult = await _mediator.Send(
                new GetNextOrderIndexQuery(request.QuizId),cancellationToken
            );
            if (!orderResult.IsSuccess)
                return RequestResult<AddQuestionResponse>.Failure("Failed to get next order index");


            // 2. Create Question
            var question = new Question
            {
                QuizId = request.QuizId,
                Text = request.Text,
                Explanation = request.Explanation,
                OrderIndex = orderResult.Data.NextOrderIndex 
            };

            var questionRepo = _unitOfWork.GetRepository<Question>();
            questionRepo.Add(question);
            // Save the question first to generate its unique Id (Primary Key) from the database, 
            // which is required to link the incoming options to this specific question.
            await _unitOfWork.SaveChangesAsync();

            // 3. Create Options
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

            return RequestResult<AddQuestionResponse>
                .Success(new AddQuestionResponse(question.Id), "Question created successfully.");
        }

    }

}