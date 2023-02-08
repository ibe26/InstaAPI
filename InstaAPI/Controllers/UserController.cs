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
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

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
            if (await uow.UserRepository.AnyAsync(u => u.Nickname == registerUser.NickName || u.Email == registerUser.Email))
            {
                return BadRequest("User Alrady Exists.");
            }
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
            //First check if user exists. If not, break.
            //Then Check if User Token exists. If exists,Check passwords. If doesn't match, break. If matches, return current token.
            //If User Token doesn't exists, check for password validation.If matches, create new token and return the current.

            User FoundUser = await uow.UserRepository.SingleOrDefault(u => u.Email == user.Email);
            if (FoundUser is null)
            {
                return BadRequest("User doesn't exist.");
            }
            if (await uow.UserRepository.AnyAsync(u => u.UserID == FoundUser.UserID))
            {
                if (!MatchPassword(user.Password, FoundUser.Password, FoundUser.PasswordKey))
                {
                    return BadRequest("Please Check e-mail or password entered.");
                }
            }
            string Token = CreateJWT(user);
            if (Token == null)
            {
                return BadRequest();
            }
            return Ok(new {nickname=FoundUser.Nickname,Token=Token});

        }

        //[HttpDelete("logout/{UserID}")]
        //public async Task<IActionResult> Logout(int UserID)
        //{
        //    if (!await uow.UserTokenRepository.AnyAsync(u => u.UserID == UserID))
        //    {
        //        return BadRequest("User Doesn't exist.");
        //    }

        //    int ID = uow.UserTokenRepository.SingleOrDefault(u => u.UserID == UserID).Result.ID;
        //    await uow.UserTokenRepository.DeleteAsync(ID);

        //    if (await uow.SaveAsync())
        //    {
        //        return Ok();
        //    }
        //    return BadRequest();
        //}

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
