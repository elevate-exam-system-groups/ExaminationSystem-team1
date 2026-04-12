using ExaminationSystem.Controllers.AccountController.ViewModels.LoginViewModels;
using ExaminationSystem.Controllers.AccountController.ViewModels.PasswordViewModels;
using ExaminationSystem.Controllers.Shared;
using ExaminationSystem.Controllers.Shared.Enums;
using ExaminationSystem.Features.Account.ForgetResetPassword.Forgot_ResetPassword;
using ExaminationSystem.Features.AuthModule.Account.Command;
using ExaminationSystem.Features.AuthModule.Account.DTOs;
using ExaminationSystem.Features.AuthModule.UserLogin.LoginRequests.Commands;
using ExaminationSystem.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers.AccountController
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ResponseViewModel<UserDTO>> Register([FromBody] RegisterDTO registerDTO)
        {
            var result = await _mediator.Send(new RegisterCommand(registerDTO));
            return ResponseViewModel<UserDTO>.Success(result.Data);
        }

        [HttpPost]
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

        [HttpPost]
        public async Task<ResponseViewModel<ForgotPasswordResponseVm>> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            var command = new ForgotPasswordCommand(model.Email);

            var result = await _mediator.Send(command);
            return ResponseViewModel<ForgotPasswordResponseVm>.Success();

        }

        [HttpPost]
        public async Task<ResponseViewModel<ResetPasswordResponseVm>> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            var command = new ResetPasswordCommand
            (model.Token, model.NewPassword, model.ConfirmNewPassword);

            var result = await _mediator.Send(command);
            return result.ToHttpResponse();
        }

    }
}



