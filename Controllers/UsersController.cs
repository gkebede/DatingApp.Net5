using DatingApp_API.Models;
using DatingAppAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp_API.Controllers
{
    
    public class UsersController : BaseApiController
    {
        private readonly DataContext _dbContext;

        public UsersController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppUsers>>> GetUsers() 
        {
            return await  _dbContext.AppUsers.ToListAsync();
         }

       [Authorize]
        [HttpGet("{id}")]
        public async Task <ActionResult<AppUsers>> GetUser(int id)
        {
            return  await _dbContext.AppUsers.FirstOrDefaultAsync(u => u.Id == id);
        }

    }
}
