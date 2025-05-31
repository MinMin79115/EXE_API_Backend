using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EXE_API_Backend.Models.Model
{
    public class EmailHistory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("user_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonElement("to_email")]
        public string ToEmail { get; set; }

        [BsonElement("subject")]
        public string Subject { get; set; }

        [BsonElement("body")]
        public string Body { get; set; }

        [BsonElement("sent_at")]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        [BsonElement("status")]
        public string Status { get; set; } = "sent";
    }
} 