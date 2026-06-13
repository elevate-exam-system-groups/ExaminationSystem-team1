using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using ExaminationSystem.Domain.Data;
using ExaminationSystem.Domain.Models;

namespace ExaminationSystem.Features.Authentication.Commands
{
    public class RegisterStudentCommandHandler : IRequestHandler<RegisterStudentCommand, RegisterStudentResponse>
    {
        private readonly Context _context;

        public RegisterStudentCommandHandler(Context context)
        {
            _context = context;
        }

        public async Task<RegisterStudentResponse> Handle(RegisterStudentCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return new RegisterStudentResponse(false, "Email and Password are required.", string.Empty);
            }

            // Check if email already exists
            bool emailExists = await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
            if (emailExists)
            {
                return new RegisterStudentResponse(false, "Email is already registered.", string.Empty);
            }

            // Create new User entity
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                FullName = request.FullName,
                Email = request.Email,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                accountStatus = global::ExaminationSystem.ExaminationSystem.Domain.Models.Enums.AccountStatus.Active,
                EmailConfirmed = true
            };

            // Create new Student entity linked to User
            var student = new Student
            {
                UserId = user.Id
            };

            // Save both
            _context.Users.Add(user);
            _context.Students.Add(student);
            await _context.SaveChangesAsync(cancellationToken);

            return new RegisterStudentResponse(true, "Student registered successfully.", user.Id);
        }
    }
}
