using System.Threading.Tasks;

namespace EXE_API_Backend.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
} 