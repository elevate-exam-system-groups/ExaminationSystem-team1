using ExaminationSystem.Domain.Data;
using ExaminationSystem.Domain.Models;
using ExaminationSystem.ExaminationSystem.Domain.Models.Enums;
using ExaminationSystem.Features.AuthModule.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace ExaminationSystem.Features.Account.Reqisteration.Command
{
    public record VerifyOtpCommand(string Email, string OtpCode) : IRequest<RequestResult<string>>;

    public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, RequestResult<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly Context _context;

        public VerifyOtpCommandHandler(UserManager<User> userManager, Context context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<RequestResult<string>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return RequestResult<string>.Failure("Invalid email or OTP", RequestErrorCode.InvalidCredentials);

            if (user.accountStatus == AccountStatus.Active && user.EmailConfirmed)
                return RequestResult<string>.Success("Account is already verified.");

            var otpHash = global::ExaminationSystem.Features.Account.ForgetResetPassword.Helper.HashToken.TokenHasher.HashToken(request.OtpCode);

            var otpRecord = await _context.UserOTPs
                .Where(x => x.UserId == user.Id && x.CodeHash == otpHash && !x.IsUsed)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);

            if (otpRecord == null)
            {
                // Increment failed attempt logic could go here
                return RequestResult<string>.Failure("Invalid OTP", RequestErrorCode.InvalidToken);
            }

            if (otpRecord.ExpiryDate < DateTime.UtcNow)
            {
                return RequestResult<string>.Failure("OTP expired", RequestErrorCode.InvalidToken);
            }

            // Valid!
            otpRecord.IsUsed = true;
            user.accountStatus = AccountStatus.Active;
            user.EmailConfirmed = true;

            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync(cancellationToken);

            return RequestResult<string>.Success("Account verified successfully.");
        }
    }
}

