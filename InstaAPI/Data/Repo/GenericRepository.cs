using InstaAPI.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace InstaAPI.Data.Repo
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DataContext dataContext;
        private DbSet<TEntity> _object;

        public GenericRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            _object = dataContext.Set<TEntity>();
        }
        public async Task DeleteAsync(int ID)
        {
            TEntity TEntity = await FindByID(ID);
            _object.Remove(TEntity);
        }

        public async Task<TEntity> FindByID(int ID)
        {
            return await _object.FindAsync(ID);

        }

        public async Task<IEnumerable<TEntity>> GetListAsync()
        {
            return await _object.ToListAsync();
        }

        public async Task InsertAsync(TEntity parameter)
        {
            await _object.AddAsync(parameter);
        }

        public Task Patch(int ID, JsonPatchDocument TEntity)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return await _object.SingleOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return await _object.Where(predicate).ToListAsync();

        }
    }
}
