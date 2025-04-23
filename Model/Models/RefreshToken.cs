using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EXE_API_Backend.Models.Model
{
    public class RefreshToken
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("token")]
        public string token { get; set; }

        [BsonElement("user_id")]
        public string userId { get; set; }

        [BsonElement("expiry_date")]
        public DateTime expiryDate { get; set; }

        [BsonElement("is_revoked")]
        public bool isRevoked { get; set; } = false;
    }
} 