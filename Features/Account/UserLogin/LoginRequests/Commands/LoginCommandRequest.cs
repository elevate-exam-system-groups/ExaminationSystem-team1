using ExaminationSystem.Domain.Models;
using ExaminationSystem.ExaminationSystem.Domain.Models.Enums;
using ExaminationSystem.Features.AuthModule.Shared;
using ExaminationSystem.Features.AuthModule.UserLogin.LoginDTOS;
using ExaminationSystem.Features.Common.Enums;
using ExaminationSystem.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace ExaminationSystem.Features.AuthModule.UserLogin.LoginRequests.Commands
{
    #region Request

    public record LoginCommandRequest(string Email, string Password) : IRequest<RequestResult<LoginCommandResponseDTO>>
    {
    }

    #endregion

    #region Validator

    public class LoginCommandValidator : AbstractValidator<LoginCommandRequest>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(5).WithMessage("Password must be at least 5 characters");
        }
    }
    #endregion

    public class LoginCommandHandler : IRequestHandler<LoginCommandRequest, RequestResult<LoginCommandResponseDTO>>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly Context _context;

        public LoginCommandHandler(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ITokenGenerator tokenGenerator,
            Context context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenGenerator = tokenGenerator;
            _context = context;
        }

        public async Task<RequestResult<LoginCommandResponseDTO>> Handle(
            LoginCommandRequest request,
            CancellationToken cancellationToken)
        {
            var validator = new LoginCommandValidator();
            var resultValidate = await validator.ValidateAsync(request);

            if (!resultValidate.IsValid)
            {
                return RequestResult<LoginCommandResponseDTO>
                     .Failure("Invalid email or password", RequestErrorCode.InvalidCredentials);
            }

            // 1. Find User
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return RequestResult<LoginCommandResponseDTO>
                    .Failure("Invalid email or password", RequestErrorCode.InvalidCredentials);

            // 2. Check AccountStatus
            if (user.accountStatus != AccountStatus.Active)
                return RequestResult<LoginCommandResponseDTO>
                    .Failure("Account is not active", RequestErrorCode.AccountNotActive);

            // 3. Check Email Confirmed
            if (!user.EmailConfirmed)
                return RequestResult<LoginCommandResponseDTO>
                    .Failure("Email not verified", RequestErrorCode.EmailNotVerified);

            // 4. Check Account Locked
            if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow)
                return RequestResult<LoginCommandResponseDTO>
                    .Failure("Account is locked, try again later", RequestErrorCode.AccountLocked);

            // 5. Check Password
            var signInResult = await _signInManager
                .CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

            if (!signInResult.Succeeded)
                return RequestResult<LoginCommandResponseDTO>
                    .Failure("Invalid email or password", RequestErrorCode.InvalidCredentials);

            // 6. Generate Access Token
            var token = _tokenGenerator.GenerateAccessToken(user);

            // 7. Generate Refresh Token
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                JwtId = Guid.NewGuid().ToString(),
                IsUsed = false,
                IsRevoked = false,
                UserId = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            };
            
            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync(cancellationToken);

            return RequestResult<LoginCommandResponseDTO>.Success(new LoginCommandResponseDTO
            {
                Email = user.Email,
                Token = token,
                RefreshToken = refreshToken
            });
        }

    }
}
