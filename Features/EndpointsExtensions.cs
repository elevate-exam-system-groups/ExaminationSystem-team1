using ExaminationSystem.Features.Account.ForgetResetPassword;
using ExaminationSystem.Features.Account.Reqisteration;
using ExaminationSystem.Features.Account.UserLogin;
using ExaminationSystem.Features.DiplomaFeatures.CreateDiploma;
using ExaminationSystem.Features.DiplomaFeatures.DeleteDiploma;
using ExaminationSystem.Features.DiplomaFeatures.GetAllDiplomas;
using ExaminationSystem.Features.DiplomaFeatures.GetAllDiplomasAllStatuses;
using ExaminationSystem.Features.DiplomaFeatures.GetDiplomaWithQuizzesForLoggedStudent;
using ExaminationSystem.Features.DiplomaFeatures.UpdateDiploma;

namespace ExaminationSystem.Features
{
    public static class EndpointsExtensions
    {
        public static void MapAllEndpoints(this IEndpointRouteBuilder app)
        {
            // Diploma Endpoints
            app.MapCreateDiplomaEndpoint();
            app.MapUpdateDiplomaEndpoint();
            app.MapDeleteDiplomaEndpoint();
            app.MapGetAllDiplomasEndpoint();
            app.MapGetAllDiplomasAllStatusesEndpoint();
            app.MapGetDiplomaQuizzesEndpoint();

            // Auth Endpoints
            app.MapRegisterEndpoint();
            app.MapVerifyOtpEndpoint();
            app.MapResendOtpEndpoint();
            app.MapLoginEndpoint();
            app.MapForgotPasswordEndpoint();
            app.MapResetPasswordEndpoint();
        }
    }
}
