using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaAPI.Interfaces
{
    public interface IGenericRepository<T>
    {
        public Task<IEnumerable<T>> GetListAsync();
        public Task InsertAsync(T parameter);
        public Task DeleteAsync(int ID);
        public Task<T> FindByID(int ID);
    }
}
