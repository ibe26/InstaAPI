using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InstaAPI.Model;

namespace InstaAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<WhoLiked> WhoLiked { get; set; }
        public DbSet<UserToken> UserToken { get; set; }
    }
}
