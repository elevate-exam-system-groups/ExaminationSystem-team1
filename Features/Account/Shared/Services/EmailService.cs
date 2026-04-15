using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace ExaminationSystem.Features.Account.Shared.Services
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendPasswordResetEmailAsync(string email, string token, string userName)
        {
            try
            {
                var resetLink = $"{_configuration["AppSettings:ClientUrl"]}" +
                    $"/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(email)}";

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress
                    ("Examination System", _configuration["EmailSettings:SenderEmail"]));
                message.To.Add(new MailboxAddress(userName, email));
                message.Subject = "Password Reset Request";

                message.Body = new TextPart("html")
                {
                    Text = $@"
                        <html>
                        <body>
                            <h2>Password Reset Request</h2>
                            <p>Hello {userName},</p>
                            <p>We received a request to reset your password. Click the link below to reset it:</p>
                            <p><a href='{resetLink}'>Reset Password</a></p>
                            <p>This link will expire in 15 minutes.</p>
                            <p>If you didn't request this, please ignore this email.</p>
                            <br/>
                            <p>Best regards,<br/>Examination System Team</p>
                        </body>
                        </html>"
                };

                // print token before Hashing
                Console.WriteLine($"DEBUG_TOKEN: {token}");

                using var client = new SmtpClient();
                await client.ConnectAsync(
                    _configuration["EmailSettings:SmtpServer"],
                    int.Parse(_configuration["EmailSettings:SmtpPort"]),
                    SecureSocketOptions.StartTls);

                await client.AuthenticateAsync(
                    _configuration["EmailSettings:SmtpUsername"],
                    _configuration["EmailSettings:SmtpPassword"]);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                // Use ILogger here instead of Console
                Console.WriteLine($"Email sending failed: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendVerificationEmailAsync(string email, string otp, string userName)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Examination System", _configuration["EmailSettings:SenderEmail"]));
                message.To.Add(new MailboxAddress(userName, email));
                message.Subject = "Account Verification OTP";

                message.Body = new TextPart("html")
                {
                    Text = $@"
                        <html>
                        <body>
                            <h2>Account Verification</h2>
                            <p>Hello {userName},</p>
                            <p>Your one-time verification code is: <strong>{otp}</strong></p>
                            <p>This code will expire in 10 minutes.</p>
                            <br/>
                            <p>Best regards,<br/>Examination System Team</p>
                        </body>
                        </html>"
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(
                    _configuration["EmailSettings:SmtpServer"],
                    int.Parse(_configuration["EmailSettings:SmtpPort"]),
                    SecureSocketOptions.StartTls);

                await client.AuthenticateAsync(
                    _configuration["EmailSettings:SmtpUsername"],
                    _configuration["EmailSettings:SmtpPassword"]);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OTP Email sending failed: {ex.Message}");
                return false;
            }
        }


    }
}