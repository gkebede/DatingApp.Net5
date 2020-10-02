using DatingApp_API.DTOs;
using DatingApp_API.interfaces;
using DatingApp_API.Models;
using DatingAppAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp_API.Controllers
{
    public class AccountController: BaseApiController
    {
        private readonly DataContext _dataContext;
        private readonly ITokenService _tokenService;

        public AccountController(DataContext dataContext, ITokenService tokenService)
        {
            _dataContext = dataContext;
            _tokenService = tokenService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto  registerDto) 
        {
            if (string.IsNullOrEmpty(registerDto.Username)) 
            {
                return NotFound("User Name is required");
            }

            if (await UserExists(registerDto.Username))
            {
                ModelState.AddModelError("User Name ", registerDto.Username + " is already taken");
                return BadRequest(ModelState);
            }
            //byte[] passwordHash, passwordSals;
            using var hmac = new HMACSHA512();

            var user = new AppUsers
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            
           _dataContext.AppUsers.Add(user); 

          await  _dataContext.SaveChangesAsync();

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto  loginDto)
        {

            var user = await _dataContext.AppUsers
                       .FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
            if (user == null)
            {
                return Unauthorized("Invalid username");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);
             
              var coputedHash =  hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < coputedHash.Length; i++) 
            {
                if (coputedHash[i] != user.PasswordHash[i]) 
                {
                    return Unauthorized("Invalid password");
                }
            }

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };

        }

        private async Task<bool> UserExists(string userName)
        {
           return await _dataContext.AppUsers.AnyAsync(x => x.UserName == userName.ToLower());
        }
    }
}
