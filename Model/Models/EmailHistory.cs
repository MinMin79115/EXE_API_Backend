using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EXE_API_Backend.Models.Model
{
    public class EmailHistory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("to_email")]
        public string toEmail { get; set; }

        [BsonElement("subject")]
        public string subject { get; set; }

        [BsonElement("body")]
        public string body { get; set; }

        [BsonElement("sent_at")]
        public DateTime sentAt { get; set; } = DateTime.UtcNow;

        [BsonElement("is_sent")]
        public bool isSent { get; set; } = false;

        [BsonElement("error_message")]
        public string errorMessage { get; set; } = string.Empty;
    }
} 