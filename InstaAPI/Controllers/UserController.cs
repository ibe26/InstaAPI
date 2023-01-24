using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstaAPI.Model.DTOs;
using InstaAPI.Model;
using InstaAPI.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace InstaAPI.Controllers
{
    [ApiController]
    [Route("/api/{controller}")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork uow;
        public UserController(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var users = await uow.UserRepository.GetListAsync();
            return Ok(users.Select(u => new
            {
                u.UserID,
                u.Nickname,
                u.Email,
            }));
        }

        [HttpGet("{UserID}")]
        public async Task<IActionResult> UserWithPosts(int UserID)
        {
            User User = await uow.UserRepository.FindByID(UserID);
            IEnumerable<Post> Posts = await uow.PostRepository.Where(p => p.UserID == UserID);
            User.Posts = Posts.ToList();

            var UserInfo = new
            {
                User.UserID,
                User.Nickname,
                User.Email,
                User.Posts
            };
            return Ok(UserInfo);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerUser)
        {
            byte[] passwordHash, passwordKey;
            using (var hmac = new HMACSHA256())
            {
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(registerUser.Password));
            }

            var user = new User
            {
                Email = registerUser.Email,
                Nickname = registerUser.NickName,
                Password = passwordHash,
                PasswordKey = passwordKey,
            };
            await uow.UserRepository.InsertAsync(user);
            await uow.SaveAsync();
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO user)
        {
            var FoundUser = await uow.UserRepository.SingleOrDefault(u => u.Email == user.Email);
            if (FoundUser is null)
            {
                return BadRequest("User doesn't exist.");
            }
            if (!MatchPassword(user.Password, FoundUser.Password, FoundUser.PasswordKey))
            {
                return BadRequest("Please Check e-mail or password entered.");
            }

            User result = await uow.UserRepository.SingleOrDefault(u => u.Email == user.Email);

            return Ok(new
            {
                PostID = result.UserID,
                Token = CreateJWT(user)
            });
        }

        private string CreateJWT(LoginDTO user)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(user.Password + "TOKENTEST123123123123123"));
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Email,user.Email)
            };

            var signingCredantials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = signingCredantials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private bool MatchPassword(string passwordText, byte[] UserPaswword, byte[] PasswordKey)
        {
            using (var hmac = new HMACSHA256(PasswordKey))
            {
                var passwordKey = hmac.Key;
                var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passwordText));
                for (int i = 0; i < passwordHash.Length; i++)
                {
                    if (passwordHash[i] != UserPaswword[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

    }
}
