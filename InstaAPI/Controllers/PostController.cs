using InstaAPI.Interfaces;
using InstaAPI.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IUnitOfWork uow;

        public PostController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            IEnumerable<Post> posts = await uow.PostRepository.GetListAsync();
            IEnumerable<User> users = await uow.UserRepository.GetListAsync();
            IEnumerable<WhoLiked> whoLikeds = await uow.WhoLikedRepository.GetListAsync();
            var result = from post in posts
                         join user in users
                         on post.UserID equals user.UserID
                         select new
                         {
                             post.Date,
                             post.ID,
                             post.Image,
                             post.Text,
                             post.Title,
                             post.WhoLiked,
                             user.Nickname
                         };
            return Ok(result);
        }

        public Post GetPost(PostDTO postDTO)
        {
            return new Post()
            {
                Date = DateTime.Now.Date,
                Image = postDTO.Image,
                Text = postDTO.Text,
                Title = postDTO.Title,
                UserID = postDTO.UserID,
            };
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddPost(PostDTO postDTO)
        {
            Post post = new Post()
            {
                Date = DateTime.Now.Date,
                Image = postDTO.Image,
                Text = postDTO.Text,
                Title = postDTO.Title,
                UserID = postDTO.UserID,
                WhoLiked = null

            };
            await uow.PostRepository.InsertAsync(post);
            return Ok(await uow.SaveAsync());

        }
        ///api/post/like?PostID=1&UserID=5
        [HttpPost("like")]
        public async Task<IActionResult> LikePost(int UserID, int PostID)
        {
            if (!await uow.PostRepository.AnyAsync(p => p.ID == PostID))
            {
                return BadRequest("Attempted to like a post that doesn't exist.");
            }
            if (!await uow.UserRepository.AnyAsync(u => u.UserID == UserID))
            {
                return BadRequest("A non-existing user tried to like a post");
            }

            if (!await uow.WhoLikedRepository.AnyAsync(p => p.UserID == UserID && p.PostID == PostID))
            {
                var whoLiked = new WhoLiked() { PostID = PostID, UserID = UserID };

                await uow
                .WhoLikedRepository
                .InsertAsync(whoLiked);
                return Ok(await uow.SaveAsync());
            }
            return BadRequest("Either a non-existing user tried to like or the post doesn't exist.");
        }

        [HttpPost("unlike")]
        public async Task<IActionResult> UnlikePost(int UserID, int PostID)
        {
            //Checking if this UserID liked this post before

            if (await uow.WhoLikedRepository.AnyAsync(p => p.UserID == UserID && p.PostID == PostID))
            {
                var whoLiked = uow
                .WhoLikedRepository
                .SingleOrDefault(p => p.UserID == UserID && p.PostID == PostID)
                .Result
                .ID;

                await uow
                .WhoLikedRepository
                .DeleteAsync(whoLiked);

                if (await uow.SaveAsync())
                {
                    return Ok(200);
                }

            }
            return BadRequest("Attempted to unlike the post, even though didn't like.");
        }
    }
}
