using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Features.Account.DTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExaminationSystem.Features.Account.Command
{
    public record RegisterCommand(RegisterDTO registerDTO) : IRequest<RequestResult<UserDTO>>;

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RequestResult<UserDTO>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public RegisterCommandHandler(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<RequestResult<UserDTO>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // 1. Check if the email is already registered

            var existingUser = await _userManager.FindByEmailAsync(request.registerDTO.Email);
            if(existingUser is not null)
                return new RequestResult<UserDTO>(null!, false, ErrorCode.EmailAlreadyRegistered);

            var user = new User
            {
                UserName = request.registerDTO.Email,
                Email = request.registerDTO.Email,
                FullName = request.registerDTO.FullName,
            };

            var identityResult= await _userManager.CreateAsync(user, request.registerDTO.Password);

            if (!identityResult.Succeeded)
            {
                var errors = identityResult.Errors.Select(e => e.Description).ToList();
                return new RequestResult<UserDTO>(null!, false, ErrorCode.PasswordPolicyViolation);
            }

            var token = await CreateTokenAsync(user);
            return new RequestResult<UserDTO>(new UserDTO(user.Email, user.FullName, token), true, ErrorCode.Success);

        }

        private async Task<string> CreateTokenAsync(User user)
        {
            //Token [Issure, Audience, Expiry, SigningCredentials, claims]

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName!)
            };

            var rols = await _userManager.GetRolesAsync(user);
            foreach (var role in rols)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var SecretKey = _configuration["JwtOptions:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtOptions:Issuer"],
                audience: _configuration["JwtOptions:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
