using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IServices;
using Application.Entities;
using System.Security.Cryptography;

namespace Application.Services
{
    public class UserService : IUserService
    {
        protected readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public User LoginUser(string email, string password)
        {
            User user = _context.Users.Where(u => u.Email == email).FirstOrDefault();

            if (user == null) return null;

            string savedPasswordHash = user.Password;
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return null;

            return user;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users;
        }
        public User AddUser(User user)
        {
            #region PasswordHashing
            string password = user.Password;
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            user.Password = Convert.ToBase64String(hashBytes);
            #endregion
            _context.Add(user);
            _context.SaveChanges();
            return user;
        }
        public void RemoveUser(int id)
        {
            var user = _context.Find<User>(id);
            if(user != null)
            {
                _context.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}
