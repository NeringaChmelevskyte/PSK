using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Entities;

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

    }
}
