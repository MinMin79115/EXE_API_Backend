using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EXE_API_Backend.Models.Model
{
    public enum Role
    {
        Admin,
        User
    }

    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [BsonElement("user_name")]
        public string userName { get; set; }

        [BsonElement("password_hash")]
        public string passwordHash { get; set; }

        [BsonElement("email")]
        public string email { get; set; }

        [BsonElement("full_name")]
        public string fullName { get; set; }

        [BsonElement("role")]
        [BsonRepresentation(BsonType.String)]
        public Role role { get; set; } = Role.User;

        [BsonElement("created_at")]
        public DateTime createdAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updated_at")]
        public DateTime updatedAt { get; set; } = DateTime.UtcNow;
    }
} 