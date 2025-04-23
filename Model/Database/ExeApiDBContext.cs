using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System;
using EXE_API_Backend.Models.Model;

namespace EXE_API_Backend.Models.Database
{
    public class ExeApiDBContext
    {
        private readonly IMongoDatabase _database;

        private readonly IMongoCollection<User> _users;
        private readonly IMongoCollection<RefreshToken> _refreshTokens;
        private readonly IMongoCollection<EmailHistory> _emailHistories;

        public ExeApiDBContext(IOptions<DatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);

            _users = _database.GetCollection<User>("Users");
            _refreshTokens = _database.GetCollection<RefreshToken>("RefreshTokens");
            _emailHistories = _database.GetCollection<EmailHistory>("EmailHistories");
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }

        public IMongoCollection<User> Users => _users;
        public IMongoCollection<RefreshToken> RefreshTokens => _refreshTokens;
        public IMongoCollection<EmailHistory> EmailHistories => _emailHistories;

        public IMongoDatabase GetDatabase() => _database;
    }
} 