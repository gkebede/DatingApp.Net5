using DatingAppAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp_API.interfaces
{
   public interface ITokenService
    {
        string CreateToken(AppUsers  user);
    }
}
