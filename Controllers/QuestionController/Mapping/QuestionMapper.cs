using ExaminationSystem.Controllers.QuestionController.ViewModels.Add;
using ExaminationSystem.Controllers.QuestionController.ViewModels.Delete;
using ExaminationSystem.Controllers.QuestionController.ViewModels.Update;
using ExaminationSystem.Features.Questions_OptionsModule.AddQuestion.Orchestrator;
using ExaminationSystem.Features.Questions_OptionsModule.UpdateQuestion.Orchestrator;

namespace ExaminationSystem.Controllers.QuestionController.Mapping
{
    public static class QuestionMapper
    {
        public static AddQuestionResponseVM ToViewModel(this AddQuestionResponseDto dto)
            => new AddQuestionResponseVM(dto.added);

        public static UpdateQuestionResponseVM ToViewModel(this UpdateQuestionResponseDto dto)
            => new UpdateQuestionResponseVM(dto.updated);

        public static DeleteQuestionResponseVM ToViewModel(this DeleteResponseDto dto)
            => new DeleteQuestionResponseVM(dto.Deleted);

        // =========
        public static List<OptionDto> ToDto(this List<OptionVM> vms)
            => vms.Select(o => new OptionDto(o.Text, o.IsCorrect)).ToList();

        public static List<UpdateOptionDto> ToDto(this List<UpdateOptionVM> vms)
            => vms.Select(o => new UpdateOptionDto(o.Id, o.Text, o.IsCorrect)).ToList();

        
        public static AddQuestionOrchestratorCommand ToCommand(this AddQuestionVM vm, Guid quizId)
        {
            return new AddQuestionOrchestratorCommand
            {
                QuizId = quizId,
                Text = vm.Text,
                Explanation = vm.Explanation,
                Options = vm.Options.Select(o => new OptionDto(o.Text, o.IsCorrect)).ToList()
            };
        }

        public static UpdateQuestionOptionsOrchestrator ToCommand(this UpdateQuestionVM vm, Guid id)
        {
            return new UpdateQuestionOptionsOrchestrator
            {
                Id = id,
                Text = vm.Text,
                Explanation = vm.Explanation,
                Options = vm.Options.Select(o => new UpdateOptionDto(o.Id, o.Text, o.IsCorrect)).ToList()
            };
        }

    }
}