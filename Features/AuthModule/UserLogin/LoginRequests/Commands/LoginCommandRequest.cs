using ExaminationSystem.ExaminationSystem.Domain.Models.Enums;
using ExaminationSystem.Features.AuthModule.UserLogin.LoginDTOS;
using ExaminationSystem.Features.Common;
using ExaminationSystem.Features.Common.Enums;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
                .MinimumLength(8).WithMessage("Password must be at least 8 characters");
        }
    }
    #endregion

    public class LoginCommandHandler : IRequestHandler<LoginCommandRequest, RequestResult<LoginCommandResponseDTO>>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public LoginCommandHandler(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<RequestResult<LoginCommandResponseDTO>> Handle(
            LoginCommandRequest request,
            CancellationToken cancellationToken)
        {
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

            // 6. Generate Token
            var token = GenerateAccessToken(user);

            return RequestResult<LoginCommandResponseDTO>.Success(new LoginCommandResponseDTO
            {
                Email = user.Email,
                Token = token
            });
        }

        private string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(15),
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
