using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SehirRehberi.Api.Models;

namespace SehirRehberi.Api.Data
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);
         Task<User> Login(string Username, string password);
         Task<bool> UserExists(string UserName);

    }
}
