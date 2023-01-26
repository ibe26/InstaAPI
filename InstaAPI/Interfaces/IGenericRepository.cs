using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace InstaAPI.Interfaces
{
    public interface IGenericRepository<TEntity>
    {
        public Task<IEnumerable<TEntity>> GetListAsync();
        public Task InsertAsync(TEntity parameter);
        public Task DeleteAsync(int ID);
        public Task<TEntity> FindByID(int ID);
        public Task<IEnumerable<TEntity>> Where(Expression<Func<TEntity, bool>> predicate);
        public Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> predicate);
        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        public Task Patch(int ID, JsonPatchDocument TEntity);
    }
}
