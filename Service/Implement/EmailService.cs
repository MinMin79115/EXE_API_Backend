using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using EXE_API_Backend.Models.Model;
using EXE_API_Backend.Repositories.Interface;

namespace EXE_API_Backend.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailTemplateService _emailTemplateService;

        public EmailService(IConfiguration configuration, IUnitOfWork unitOfWork, EmailTemplateService emailTemplateService)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _emailTemplateService = emailTemplateService;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var smtpUsername = _configuration["EmailSettings:SmtpUsername"];
            var smtpPassword = _configuration["EmailSettings:SmtpPassword"];
            var fromEmail = _configuration["EmailSettings:FromEmail"];

            using var client = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            try
            {
                await client.SendMailAsync(mailMessage);

                // Lưu lịch sử gửi email
                var emailHistory = new EmailHistory
                {
                    toEmail = toEmail,
                    subject = subject,
                    body = body,
                    isSent = true
                };
                await _unitOfWork.EmailHistoryRepository.AddAsync(emailHistory);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                // Lưu lịch sử gửi email với thông tin lỗi
                var emailHistory = new EmailHistory
                {
                    toEmail = toEmail,
                    subject = subject,
                    body = body,
                    isSent = false,
                    errorMessage = ex.Message
                };
                await _unitOfWork.EmailHistoryRepository.AddAsync(emailHistory);
                await _unitOfWork.CommitAsync();
                throw;
            }
        }

        // Phương thức bổ sung để gửi email xác nhận đăng ký
        public async Task SendRegistrationConfirmationEmail(string toEmail, string fullName, string userName)
        {
            var subject = "Xác nhận đăng ký tài khoản";
            var body = _emailTemplateService.GetRegistrationConfirmationTemplate(fullName, userName);
            await SendEmailAsync(toEmail, subject, body);
        }

        // Phương thức bổ sung để gửi email đặt lại mật khẩu
        public async Task SendPasswordResetEmail(string toEmail, string fullName, string resetLink)
        {
            var subject = "Đặt lại mật khẩu";
            var body = _emailTemplateService.GetPasswordResetTemplate(fullName, resetLink);
            await SendEmailAsync(toEmail, subject, body);
        }

        // Phương thức bổ sung để gửi email thông báo chung
        public async Task SendGeneralNotificationEmail(string toEmail, string fullName, string message)
        {
            var subject = "Thông báo từ EXE API Backend";
            var body = _emailTemplateService.GetGeneralNotificationTemplate(fullName, message);
            await SendEmailAsync(toEmail, subject, body);
        }
    }
} 