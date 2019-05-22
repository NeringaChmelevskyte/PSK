using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.IServices
{
    public interface IUserService
    {
        User LoginUser(string email, string password);
        User AddUser(User user);
        IEnumerable<User> GetAllUsers();
        User GetUser(int id);
        void RemoveUser(int id);
        void UpdateUser(User user);
        void AddToken(User user, string token);
        User GetUserFromToken(string token);
        User GetUserFromRequest(HttpRequest Request);
        void DeleteToken(User user);

    }
}
