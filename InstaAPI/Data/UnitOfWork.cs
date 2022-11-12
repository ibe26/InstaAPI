using InstaAPI.Data.Repo;
using InstaAPI.Interfaces;
using InstaAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaAPI.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext dataContext;

        public UnitOfWork(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public IGenericRepository<Post> PostRepository => new GenericRepository<Post>(dataContext);

        public IGenericRepository<Comment> CommentRepository => new GenericRepository<Comment>(dataContext);
        public IGenericRepository<WhoLiked> WhoLikedRepository => new GenericRepository<WhoLiked>(dataContext);

        public async Task<bool> SaveAsync()
        {
            return await dataContext.SaveChangesAsync()>0;
        }
    }
}
