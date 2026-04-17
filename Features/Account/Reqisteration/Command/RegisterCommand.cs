using ExaminationSystem.Domain.Data;
using ExaminationSystem.Domain.Models;
using ExaminationSystem.ExaminationSystem.Domain.Models.Enums;
using ExaminationSystem.Features.Account.Shared.Services;
using ExaminationSystem.Features.AuthModule.Account.DTOs;
using ExaminationSystem.Features.AuthModule.Shared;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace ExaminationSystem.Features.AuthModule.Account.Command
{
    public record RegisterCommand(RegisterDTO registerDTO) : IRequest<RequestResult<string>>;

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RequestResult<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly Context _context;

        public RegisterCommandHandler(UserManager<User> userManager, IEmailService emailService, Context context)
        {
            _userManager = userManager;
            _emailService = emailService;
            _context = context;
        }

        public async Task<RequestResult<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.registerDTO.Email);
            if (existingUser is not null)
                return RequestResult<string>.Failure("Email already registered", RequestErrorCode.EmailAlreadyRegistered);

            var user = new User
            {
                UserName = request.registerDTO.Email,
                Email = request.registerDTO.Email,
                FullName = request.registerDTO.FullName,
                accountStatus = AccountStatus.Pending, // Epic 1.1 Requirements
                EmailConfirmed = false
            };

            var identityResult = await _userManager.CreateAsync(user, request.registerDTO.Password);

            if (!identityResult.Succeeded)
            {
                var errors = string.Join("; ", identityResult.Errors.Select(e => e.Description));
                return RequestResult<string>.Failure(errors, RequestErrorCode.PasswordPolicyViolation);
            }

            // Generate 6-digit OTP
            string otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
            
            // Hash the OTP before storing
            string otpHash = global::ExaminationSystem.Features.Account.ForgetResetPassword.Helper.HashToken.TokenHasher.HashToken(otp);

            var userOtp = new UserOTP
            {
                UserId = user.Id,
                CodeHash = otpHash,
                ExpiryDate = DateTime.UtcNow.AddMinutes(10), // Epic 1.4: 10 minute TTL
                IsUsed = false
            };

            _context.UserOTPs.Add(userOtp);
            await _context.SaveChangesAsync(cancellationToken);

            // Send Email
            await _emailService.SendVerificationEmailAsync(user.Email, otp, user.FullName);

            return RequestResult<string>.Success("User registered successfully. Please check your email for the verification OTP.");
        }
    }
}

