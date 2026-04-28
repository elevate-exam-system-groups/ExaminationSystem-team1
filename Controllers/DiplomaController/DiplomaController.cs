using AutoMapper;
using ExaminationSystem.Controllers.DiplomaController.ViewModels;
using ExaminationSystem.Features.DiplomaFeatures.CreateDiploma.Commands;
using ExaminationSystem.Features.DiplomaFeatures.DeleteDiploma.Commands;
using ExaminationSystem.Features.DiplomaFeatures.GetAllDiplomas.Queries;
using ExaminationSystem.Features.DiplomaFeatures.GetDiplomaWithQuizzesForLoggedStudent.Orchestrators;
using ExaminationSystem.Features.DiplomaFeatures.UpdateDiploma.Commands;


namespace ExaminationSystem.Controllers.DiplomaController
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DiplomaController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public DiplomaController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ResponseViewModel<CreateDiplomaResponseVM>> CreateDiploma(CreateDiplomaRequestVM request)
        {
            var result = await _mediator
                .Send(new CreateDiplomaCommandRequest(request.Title, request.Description));
            if (!result.IsSuccess)
            {
                return ResponseViewModel<CreateDiplomaResponseVM>
                      .Failure(result.Message, (ResponseVmErrorCode?)result.requestErrorCode);
            }

            var reponseVM = new CreateDiplomaResponseVM(
                result.Data.Id,
                result.Data.Title,
                result.Data.Description,
                result.Data.Status);

            return ResponseViewModel<CreateDiplomaResponseVM>
                 .Success(reponseVM);
        }

        [HttpPut]
        public async Task<ResponseViewModel<UpdateDiplomaResponseVM>> UpdateDiploma(UpdateDiplomaRequestVM request)
        {
            var result = await _mediator
                 .Send(new UpdateDiplomaCommandRequest(request.Id, request.Title, request.Description));
            if (!result.IsSuccess)
            {
                return ResponseViewModel<UpdateDiplomaResponseVM>
                      .Failure(result.Message, (ResponseVmErrorCode?)result.requestErrorCode);
            }

            var reponseVM = new UpdateDiplomaResponseVM(result.Data.Id, result.Data.Title, result.Data.Description, result.Data.Status);
            return ResponseViewModel<UpdateDiplomaResponseVM>
                 .Success(reponseVM);
        }

        [HttpDelete]
        public async Task<ResponseViewModel<bool>> DeleteDiploma(Guid diplomaID)
        {
            var result = await _mediator.Send(new DeleteDiplomaCommand(diplomaID));
            if (!result.IsSuccess)
            {
                return ResponseViewModel<bool>
                      .Failure(result.Message, (ResponseVmErrorCode?)result.requestErrorCode);
            }
            return ResponseViewModel<bool>
                 .Success(result.Data);
        }

        [HttpGet]
        public async Task<ResponseViewModel<GetAllDiplomasPaginatedResponseVM>> GetAllDiplomas([FromQuery] AllDiplomasPaginatedRequestVM requestVM)
        {
            var result = await _mediator.Send(new GetAllDiplomasQuery(requestVM.Page, requestVM.PerPage));
            if (!result.IsSuccess)
            {
                return ResponseViewModel<GetAllDiplomasPaginatedResponseVM>
                     .Failure(result.Message, (ResponseVmErrorCode?)result.requestErrorCode);
            }

            var responseVM = new GetAllDiplomasPaginatedResponseVM(
                result.Data.Data,
                result.Data.Page,
                result.Data.PerPage,
                result.Data.Total,
                result.Data.TotalPages
            );


            return ResponseViewModel<GetAllDiplomasPaginatedResponseVM>.Success(responseVM);
        }

        [HttpGet]
        public async Task<ResponseViewModel<List<GetDiplomaQuizzesForLoggedStudenResponseVM>>> GetDiplomaQuizzesForLoggedStudent(Guid diplomaRequestID, string StudentId)
        {
            var requestResponse = await _mediator
                .Send(new GetDiplomaQuizzesForLoggedStudentOrchestrator(diplomaRequestID, StudentId));

            if (!requestResponse.IsSuccess)
            {
                return ResponseViewModel<List<GetDiplomaQuizzesForLoggedStudenResponseVM>>
                    .Failure(requestResponse.Message, (ResponseVmErrorCode?)requestResponse.requestErrorCode);
            }

            var responseVM = _mapper
                .Map<List<GetDiplomaQuizzesForLoggedStudenResponseVM>>(requestResponse.Data);

            return ResponseViewModel<List<GetDiplomaQuizzesForLoggedStudenResponseVM>>.Success(responseVM);
        }

    }

}
