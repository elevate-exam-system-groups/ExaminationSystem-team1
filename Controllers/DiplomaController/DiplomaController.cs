using ExaminationSystem.Controllers.DiplomaController.ViewModels;
using ExaminationSystem.Controllers.Shared;
using ExaminationSystem.Controllers.Shared.Enums;
using ExaminationSystem.Features.DiplomaModule.CreateDiploma.Requests;
using ExaminationSystem.Features.DiplomaModule.DeleteDiploma.Requests;
using ExaminationSystem.Features.DiplomaModule.UpdateDiploma.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers.DiplomaController
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DiplomaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DiplomaController(IMediator mediator)
        {
            _mediator = mediator;
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

            var reponseVM = new CreateDiplomaResponseVM(result.Data.Id, result.Data.Title, result.Data.Description, result.Data.Status);

            return ResponseViewModel<CreateDiplomaResponseVM>
                 .Success(reponseVM);
        }

        [HttpPut]
        public async Task<ResponseViewModel<UpdateDiplomaResponseVM>> Update(UpdateDiplomaRequestVM request)
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
        public async Task<ResponseViewModel<bool>> Delete(Guid diplomaID)
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


    }

}
