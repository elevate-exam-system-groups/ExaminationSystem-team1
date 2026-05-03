using ExaminationSystem.Domain.Data;
using ExaminationSystem.Domain.Models;
using ExaminationSystem.ExaminationSystem.Domain.Models.Enums;
using ExaminationSystem.Features.Account.Shared.Services;
using ExaminationSystem.Features.Account.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using ExaminationSystem.Features.Common.Request;

namespace ExaminationSystem.Features.Account.Reqisteration.Command
{
    public record ResendOtpCommand(string Email) : IRequest<RequestResult<string>>;

    public class ResendOtpCommandHandler : IRequestHandler<ResendOtpCommand, RequestResult<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly Domain.Data.IUnitOfWork _context;
        private readonly IEmailService _emailService;

        public ResendOtpCommandHandler(UserManager<User> userManager, Domain.Data.IUnitOfWork context, IEmailService emailService)
        {
            _userManager = userManager;
            _context = context;
            _emailService = emailService;
        }

        public async Task<RequestResult<string>> Handle(ResendOtpCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || user.accountStatus == AccountStatus.Active)
            {
                // Return success to prevent enumeration
                return RequestResult<string>.Success("If the email is valid and pending, a new OTP has been sent.");
            }

            // Check rate limits (3 resends per hour)
            var recentOtpsCount = await _context.UserOTPs
                .Where(x => x.UserId == user.Id && x.CreatedAt >= DateTime.UtcNow.AddHours(-1))
                .CountAsync(cancellationToken);

            if (recentOtpsCount >= 3)
            {
                return RequestResult<string>.Failure("Maximum OTP resends reached for this hour.", RequestErrorCode.InvalidToken);
            }

            // Invalidate old OTPs
            var activeOtps = await _context.UserOTPs
                .Where(x => x.UserId == user.Id && !x.IsUsed && x.ExpiryDate > DateTime.UtcNow)
                .ToListAsync(cancellationToken);

            foreach (var oldOtp in activeOtps)
            {
                oldOtp.IsUsed = true;
            }

            // Generate new
            string otp = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
            string otpHash = global::ExaminationSystem.Features.Account.ForgetResetPassword.Helper.HashToken.TokenHasher.HashToken(otp);

            var userOtp = new UserOTP
            {
                UserId = user.Id,
                CodeHash = otpHash,
                ExpiryDate = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false
            };

            _context.UserOTPs.Add(userOtp);
            await _context.SaveChangesAsync(cancellationToken);

            await _emailService.SendVerificationEmailAsync(user.Email, otp, user.FullName);

            return RequestResult<string>.Success("If the email is valid and pending, a new OTP has been sent.");
        }
    }
}

