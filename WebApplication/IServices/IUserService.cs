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
        void RemoveUser(int id);
    }
}
