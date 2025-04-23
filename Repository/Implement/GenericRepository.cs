using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EXE_API_Backend.Models.Database;
using EXE_API_Backend.Repositories.Interface;

namespace EXE_API_Backend.Repositories.Implement
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly IMongoCollection<TEntity> _collection;

        public GenericRepository(ExeApiDBContext context, string collectionName)
        {
            _collection = context.GetCollection<TEntity>(collectionName);
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _collection.Find(predicate).ToListAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(string id, TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", id);
            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<TEntity>.Filter.Eq("_id", id);
            await _collection.DeleteOneAsync(filter);
        }
    }
} 