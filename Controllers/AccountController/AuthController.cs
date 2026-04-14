using ExaminationSystem.Controllers.AccountController.ViewModels.LoginViewModels;
using ExaminationSystem.Controllers.AccountController.ViewModels.PasswordViewModels;
using ExaminationSystem.Controllers.Shared;
using ExaminationSystem.Controllers.Shared.Enums;
using ExaminationSystem.Features.Account.ForgetResetPassword.Forgot_ResetPassword;
using ExaminationSystem.Features.AuthModule.Account.Command;
using ExaminationSystem.Features.AuthModule.Account.DTOs;
using ExaminationSystem.Features.AuthModule.UserLogin.LoginRequests.Commands;
using ExaminationSystem.Features.Account.Reqisteration.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers.AccountController
{
    public record OtpRequestDto(string Email, string OtpCode);
    public record ResendOtpRequestDto(string Email);

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
        public async Task<ResponseViewModel<string>> Register([FromBody] RegisterDTO registerDTO)
        {
            var result = await _mediator.Send(new RegisterCommand(registerDTO));
            if (!result.IsSuccess)
                return ResponseViewModel<string>.Failure(result.Message, ResponseVmErrorCode.InternalServerError);
            
            return ResponseViewModel<string>.Success(result.Data);
        }

        [HttpPost]
        public async Task<ResponseViewModel<string>> VerifyOtp([FromBody] OtpRequestDto request)
        {
            var result = await _mediator.Send(new VerifyOtpCommand(request.Email, request.OtpCode));
            if (!result.IsSuccess)
                return ResponseViewModel<string>.Failure(result.Message, ResponseVmErrorCode.InvalidCredentials);
            
            return ResponseViewModel<string>.Success(result.Data);
        }

        [HttpPost]
        public async Task<ResponseViewModel<string>> ResendOtp([FromBody] ResendOtpRequestDto request)
        {
            var result = await _mediator.Send(new ResendOtpCommand(request.Email));
            if (!result.IsSuccess)
                return ResponseViewModel<string>.Failure(result.Message, ResponseVmErrorCode.InternalServerError);
            
            return ResponseViewModel<string>.Success(result.Data);
        }

        [HttpPost]
        public async Task<ResponseViewModel<UserResponseVm>> Login(LoginRequestVm loginModel)
        {
            // Note: In real production, also grab IP address via HttpContext.Connection.RemoteIpAddress for logging.
            var result = await _mediator.Send(new LoginCommandRequest(loginModel.Email, loginModel.Password));

            if (!result.IsSuccess)
                return ResponseViewModel<UserResponseVm>
                    .Failure(result.Message ?? "Invalid Login", ResponseVmErrorCode.InvalidCredentials);

            // Prepend HTTP only cookie logic for RefreshToken
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
                Secure = true,   // Ensure HTTPS
                SameSite = SameSiteMode.Strict
            };
            
            Response.Cookies.Append("refreshToken", result.Data.RefreshToken, cookieOptions);

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
