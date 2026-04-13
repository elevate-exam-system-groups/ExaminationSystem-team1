using ExaminationSystem.Controllers.AccountController.ViewModels.LoginViewModels;
using ExaminationSystem.Controllers.AccountController.ViewModels.PasswordViewModels;
using ExaminationSystem.Controllers.Shared;
using ExaminationSystem.Controllers.Shared.Enums;
using ExaminationSystem.Features.Account.ForgetResetPassword.Forgot_ResetPassword;
using ExaminationSystem.Features.AuthModule.Account.Command;
using ExaminationSystem.Features.AuthModule.Account.DTOs;
using ExaminationSystem.Features.AuthModule.UserLogin.LoginRequests.Commands;
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
        public async Task<ResponseViewModel<ForgotPasswordResponseVM>> ForgotPassword([FromBody] ForgotPasswordVM model)
        {
            var command = new ForgotPasswordCommand(model.Email);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess || result.Data == null)
                return ResponseViewModel<ForgotPasswordResponseVM>.Failure(
                    result.Message ?? "An error occurred",
                    ResponseVmErrorCode.InternalServerError);

            return ResponseViewModel<ForgotPasswordResponseVM>.Success(new ForgotPasswordResponseVM
            {
                Message = result.Data.Message,
                EmailSent = result.Data.EmailSent
            });
        }

        [HttpPost]
        public async Task<ResponseViewModel<ResetPasswordResponseVM>> ResetPassword([FromBody] ResetPasswordVM model)
        {
            var command = new ResetPasswordCommand(
                model.Email,         
                model.Token,
                model.NewPassword,
                model.ConfirmNewPassword);

            var result = await _mediator.Send(command);

            if (!result.IsSuccess || result.Data == null)
                return ResponseViewModel<ResetPasswordResponseVM>.Failure(
                    result.Message ?? "Password reset failed",
                    ResponseVmErrorCode.PasswordResetFailed);

            return ResponseViewModel<ResetPasswordResponseVM>.Success(new ResetPasswordResponseVM
            {
                Success = result.Data.Success,
                Message = result.Data.Message
            });
        }
    }
}