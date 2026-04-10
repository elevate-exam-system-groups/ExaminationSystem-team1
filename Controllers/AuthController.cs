using ExaminationSystem.Controllers.ViewModels;
using ExaminationSystem.Controllers.ViewModels.Enums;
using ExaminationSystem.Controllers.ViewModels.LoginViewModels;
using ExaminationSystem.Features.AuthModule.UserLogin.LoginRequests.Commands;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMediator _mediator;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IMediator mediator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mediator = mediator;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello, World!");
        }

        [HttpPost()]
        public async Task<ResponseViewModel<UserResponseVm>> Login(LoginRequestVm loginModel)
        {
            var result = await _mediator.Send(new LoginCommandRequest(loginModel.Email, loginModel.Password));

            if (!result.IsSuccess)
                return ResponseViewModel<UserResponseVm>
                    .Failure(result.Message, ResponseVmErrorCode.InvalidCredentials);

            return ResponseViewModel<UserResponseVm>.Success(new UserResponseVm
            {
                Email = result.Data.Email,
                Token = result.Data.Token
            });

        }
    }
}
