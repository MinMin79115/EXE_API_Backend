using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EXE_API_Backend.Models.Model
{
    public enum Role
    {
        Admin,
        Customer,
        Lawyer
    }

    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("user_name")]
        public string UserName { get; set; }

        [BsonElement("password_hash")]
        public string PasswordHash { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("full_name")]
        public string FullName { get; set; }

        [BsonElement("phone")]
        public string Phone { get; set; }

        [BsonElement("role")]
        [BsonRepresentation(BsonType.String)]
        public Role Role { get; set; } = Role.Customer;

        // Lawyer specific fields
        [BsonElement("specialization")]
        public string Specialization { get; set; }

        [BsonElement("license_number")]
        public string LicenseNumber { get; set; }

        [BsonElement("years_of_experience")]
        public int? YearsOfExperience { get; set; }

        [BsonElement("bio")]
        public string Bio { get; set; }

        [BsonElement("hourly_rate")]
        public decimal? HourlyRate { get; set; }

        [BsonElement("is_active")]
        public bool IsActive { get; set; } = true;

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
} 