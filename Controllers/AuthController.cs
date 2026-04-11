using ExaminationSystem.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToHttpResponse();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return result.ToHttpResponse();
        }
    }
}