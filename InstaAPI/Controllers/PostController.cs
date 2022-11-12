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
        [HttpGet("{UserID}")]
        public async Task<IActionResult> GetPosts(int UserID)
        {
            IEnumerable<Post> posts = await uow.PostRepository.GetListAsync();
            var result = from post in posts
                         where post.UserID == UserID
                         select new
                         {
                             post.Date,
                             post.ID,
                             post.Image,
                             post.Text,
                             post.Title,
                             post.WhoLiked
                         };
            return Ok(result);
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
            };

            await uow.PostRepository.InsertAsync(post);
            await uow.SaveAsync();

            return Ok(200);

        }
        ///api/like?&PostID=1UserID=5
        [HttpPost("like")]
        public async Task<IActionResult> LikePost(int UserID, int PostID)
        {
            if(!uow.WhoLikedRepository.GetListAsync().Result.Any(p=>p.UserID==UserID && p.PostID==PostID))
            {
                var whoLiked = new WhoLiked() { PostID = PostID, UserID = UserID };

                await uow
                .WhoLikedRepository
                .InsertAsync(whoLiked);
                await uow.SaveAsync();
                return Ok(200);

            }

            return BadRequest("Either a non-existing user tried to like or the post doesn't exist.");
        }
        
        [HttpPost("unlike")]
        public async Task<IActionResult> UnlikePost(int UserID,int PostID)
        {
            //Checking if this UserID liked this post before
            
            if(uow.WhoLikedRepository.GetListAsync().Result.Any(p => p.UserID == UserID && p.PostID == PostID))
            {
                var whoLiked = uow
                .WhoLikedRepository
                .GetListAsync()
                .Result
                .Where(p => p.UserID == UserID && p.PostID == PostID)
                .FirstOrDefault()
                .ID;

                await uow
                .WhoLikedRepository
                .DeleteAsync(whoLiked);

                await uow.SaveAsync();
                return Ok(200);
            }


            return BadRequest("Attempted to unlike the post, even though didn't like.");
        }
    }
}
