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
                             post.Title
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
                LikeCount = 0
            };
            await uow.PostRepository.InsertAsync(post);
            await uow.SaveAsync();

            return Ok(200);

        }
        //[HttpPost("like/{UserID}")]
        //public async Task<IActionResult> LikePost(int UserID)
        //{

        //}
    }
}
