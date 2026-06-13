using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using ExaminationSystem.Domain.Data;

namespace ExaminationSystem.Features.Authentication.Queries
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginQueryResponse>
    {
        private readonly Context _context;

        public LoginQueryHandler(Context context)
        {
            _context = context;
        }

        public async Task<LoginQueryResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UsernameOrEmail) || string.IsNullOrEmpty(request.Password))
            {
                return new LoginQueryResponse(false, string.Empty, string.Empty, string.Empty, string.Empty);
            }

            // Find user by username or email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.UsernameOrEmail || u.UserName == request.UsernameOrEmail, cancellationToken);

            if (user == null)
            {
                return new LoginQueryResponse(false, string.Empty, string.Empty, string.Empty, string.Empty);
            }

            // Verify password using BCrypt
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                return new LoginQueryResponse(false, string.Empty, string.Empty, string.Empty, string.Empty);
            }

            // Determine role by checking presence in Admins or Students tables
            string role = "Student"; // Default role
            bool isAdmin = await _context.Admins.AnyAsync(a => a.UserId == user.Id, cancellationToken);
            if (isAdmin)
            {
                role = "Admin";
            }
            else
            {
                bool isStudent = await _context.Students.AnyAsync(s => s.UserId == user.Id, cancellationToken);
                if (!isStudent)
                {
                    // Fallback: If neither table has it but it has Admin in email, treat as Admin
                    if (user.Email.Contains("admin", System.StringComparison.OrdinalIgnoreCase))
                    {
                        role = "Admin";
                    }
                }
            }

            return new LoginQueryResponse(true, user.Id, user.UserName, user.Email, role);
        }
    }
}
