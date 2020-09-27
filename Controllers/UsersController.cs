using DatingApp_API.Models;
using DatingAppAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _dbContext;

        //private readonly DbContext _dbContext;

        public UsersController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUsers>>> GetUsers() 
        {
            return await  _dbContext.AppUsers.ToListAsync();
         }

        [HttpGet("{id}")]
        public async Task <ActionResult<AppUsers>> GetUser(int id)
        {
            return  await _dbContext.AppUsers.FirstOrDefaultAsync(u => u.Id == id);
        }

    }
}
