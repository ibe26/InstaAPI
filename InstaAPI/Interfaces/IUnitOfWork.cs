using InstaAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaAPI.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<Post> PostRepository { get; }
        public IGenericRepository<Comment> CommentRepository { get; }
        public IGenericRepository<WhoLiked> WhoLikedRepository { get; }
        public IGenericRepository<User> UserRepository { get; }
        public IGenericRepository<UserToken> UserTokenRepository { get; }
        public Task<bool> SaveAsync();
    }
}
