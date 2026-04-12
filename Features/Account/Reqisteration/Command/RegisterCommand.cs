
using ExaminationSystem.Features.AuthModule.Account.DTOs;
using ExaminationSystem.Features.AuthModule.Shared;


namespace ExaminationSystem.Features.AuthModule.Account.Command
{
    public record RegisterCommand(RegisterDTO registerDTO) : IRequest<RequestResult<UserDTO>>;

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RequestResult<UserDTO>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenGenerator _tokenGenerator;

        public RegisterCommandHandler(UserManager<User> userManager, ITokenGenerator tokenGenerator)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }
        public async Task<RequestResult<UserDTO>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // 1. Check if the email is already registered

            var existingUser = await _userManager.FindByEmailAsync(request.registerDTO.Email);
            if (existingUser is not null)
                return RequestResult<UserDTO>.Failure("", RequestErrorCode.EmailAlreadyRegistered);

            var user = new User
            {
                UserName = request.registerDTO.Email,
                Email = request.registerDTO.Email,
                FullName = request.registerDTO.FullName,
            };

            var identityResult = await _userManager.CreateAsync(user, request.registerDTO.Password);

            if (!identityResult.Succeeded)
            {
                var errors = identityResult.Errors.Select(e => e.Description).ToList();
                return RequestResult<UserDTO>.Failure("", RequestErrorCode.PasswordPolicyViolation);
            }

            var token = _tokenGenerator.GenerateAccessToken(user);
            return RequestResult<UserDTO>.Success(new UserDTO(user.Email, user.FullName, token));
        }


    }
}
