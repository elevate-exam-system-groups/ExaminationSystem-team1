using AutoMapper;
using ExaminationSystem.Controllers.DiplomaController.ViewModels;
using ExaminationSystem.Features.DiplomaModule.CreateDiploma.Requests;
using ExaminationSystem.Features.DiplomaModule.DeleteDiploma.Requests;
using ExaminationSystem.Features.DiplomaModule.GetAllDiplomas.DTOS;
using ExaminationSystem.Features.DiplomaModule.GetAllDiplomas.Request;
using ExaminationSystem.Features.DiplomaModule.GetDiplomaQuizzesForSignedInStudent;
using ExaminationSystem.Features.DiplomaModule.UpdateDiploma.Requests;

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

            var reponseVM = new CreateDiplomaResponseVM(result.Data.Id,
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
            var result = await _mediator.Send(new DeleteDiplomaCommandRequest(diplomaID));
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
            var result = await _mediator.Send(new GetDiplomasQueryRequest(requestVM.Page, requestVM.PerPage));
            if (!result.IsSuccess)
            {
                return ResponseViewModel<GetAllDiplomasPaginatedResponseVM>
                     .Failure(result.Message, (ResponseVmErrorCode?)result.requestErrorCode);
            }

            var responseVM = new GetAllDiplomasPaginatedResponseVM(
                result.Data.Data.Select(d => new GetPublishedDiplomaResponseDTO(
                    d.Id,
                    d.Title,
                    d.Description,
                    d.Status,
                    d.QuizCount
                )).ToList(),
                result.Data.Page,
                result.Data.PerPage,
                result.Data.Total,
                result.Data.TotalPages
            );

            return ResponseViewModel<GetAllDiplomasPaginatedResponseVM>.Success(responseVM);
        }

        [HttpGet]
        public async Task<ResponseViewModel<List<GetDiplomaQuizzesForSignedInStudenResponseVM>>> GetDiplomaQuizzes(Guid diplomaRequestID)
        {
            var requestResponse = await _mediator
                .Send(new GetDiplomaQuizzesForSignedInStudentQueryRequest(diplomaRequestID));

            if (!requestResponse.IsSuccess)
            {
                return ResponseViewModel<List<GetDiplomaQuizzesForSignedInStudenResponseVM>>
                    .Failure(requestResponse.Message, (ResponseVmErrorCode?)requestResponse.requestErrorCode);
            }

            var responseVM = _mapper
                .Map<List<GetDiplomaQuizzesForSignedInStudenResponseVM>>(requestResponse.Data);

            return ResponseViewModel<List<GetDiplomaQuizzesForSignedInStudenResponseVM>>.Success(responseVM);
        }

    }

}
