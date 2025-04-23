using MongoDB.Driver;
using System.Threading.Tasks;
using EXE_API_Backend.Models.Model;
using EXE_API_Backend.Repositories.Interface;
using EXE_API_Backend.Models.Database;

namespace EXE_API_Backend.Repositories.Implement
{
    public class EmailHistoryRepository : GenericRepository<EmailHistory>, IEmailHistoryRepository
    {
        public EmailHistoryRepository(ExeApiDBContext context) : base(context, "EmailHistories")
        {
        }

        public async Task<EmailHistory> GetByRecipientAndSubjectAsync(string recipientEmail, string subject)
        {
            var filter = Builders<EmailHistory>.Filter.And(
                Builders<EmailHistory>.Filter.Eq(eh => eh.toEmail, recipientEmail),
                Builders<EmailHistory>.Filter.Eq(eh => eh.subject, subject)
            );
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
    }
} 