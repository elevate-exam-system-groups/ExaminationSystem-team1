using ExaminationSystem.Controllers.ViewModels;
using ExaminationSystem.Controllers.ViewModels.Enums;
using ExaminationSystem.Controllers.ViewModels.LoginViewModels;
using ExaminationSystem.Features.Account.Command;
using ExaminationSystem.Features.Account.DTOs;
using ExaminationSystem.Features.AuthModule.UserLogin.LoginRequests.Commands;
using ExaminationSystem.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterDTO registerDTO)
        {
            var result = await _mediator.Send(new RegisterCommand(registerDTO));
            return Ok(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToHttpResponse();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToHttpResponse();
        }

        [HttpPost("Login")]
        public async Task<ResponseViewModel<UserResponseVm>> Login(LoginRequestVm loginModel)
        {
            var result = await _mediator.Send(new LoginCommandRequest(loginModel.Email, loginModel.Password));

            if (!result.IsSuccess)
                return ResponseViewModel<UserResponseVm>
                    .Failure(result.Message, ResponseVmErrorCode.InvalidCredentials);

            return ResponseViewModel<UserResponseVm>.Success(new UserResponseVm
            {
                Email = result.Data.Email,
                Token = result.Data.Token
            });

        }
    }
}