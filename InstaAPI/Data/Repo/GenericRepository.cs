using InstaAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaAPI.Data.Repo
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DataContext dataContext;
        private DbSet<T> _object;

        public GenericRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            _object = dataContext.Set<T>();
        }
        public async Task DeleteAsync(int ID)
        {
            T t = await this.FindByID(ID);
            _object.Remove(t);
        }

        public async Task<T> FindByID(int ID)
        {
            return await _object.FindAsync(ID);

        }

        public async Task<IEnumerable<T>> GetListAsync()
        {
            return await _object.ToListAsync();
        }

        public async Task InsertAsync(T parameter)
        {
            await _object.AddAsync(parameter);
        }
    }
}
