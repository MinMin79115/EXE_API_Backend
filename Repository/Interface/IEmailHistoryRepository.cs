using EXE_API_Backend.Models.Model;
using System.Threading.Tasks;

namespace EXE_API_Backend.Repositories.Interface
{
    public interface IEmailHistoryRepository : IGenericRepository<EmailHistory>
    {
        Task<EmailHistory> GetByRecipientAndSubjectAsync(string recipientEmail, string subject);
    }
} 