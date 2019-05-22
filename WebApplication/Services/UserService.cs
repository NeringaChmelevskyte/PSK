using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.IServices;
using Application.Entities;
using System.Security.Cryptography;
using WebApplication.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

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
        public User GetUser(int id)
        {
            return _context.Users.Where(i => i.Id == id).FirstOrDefault();
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

        public void UpdateUser(User user)
        {
          
            if (user != null)
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
                _context.Update(user);
                _context.SaveChanges();
            }
        }

        public void AddToken(User user, string token)
        {
            ActiveToken t = new ActiveToken
            {
                Token = token,
                UserId = user.Id
            };
            _context.Add(t);
            _context.SaveChanges();
        }

        public User GetUserFromToken(string token)
        {
            var t = new JwtSecurityTokenHandler().ReadJwtToken(token);
            if (t.ValidTo > DateTime.Now)
            {
                ActiveToken dbToken = _context.ActiveTokens.Where(i => i.Token == token).FirstOrDefault();
                if (dbToken == null)
                {
                    return null;
                }
                else
                {
                    return _context.Users.Where(i => i.Id == dbToken.UserId).FirstOrDefault();
                }
            }
            else
            {
                return null;
            }
        }

        public User GetUserFromRequest(HttpRequest Request)
        {
            string token = Request.Cookies["token"];
            if(token == null)
            {
                return null;
            }
            else
            {
                return GetUserFromToken(token);
            }
        }

        public void DeleteToken(User user)
        {
            var tokens = _context.ActiveTokens.Where(a => a.UserId == user.Id).ToList();
            foreach(ActiveToken token in tokens)
            {
                _context.ActiveTokens.Remove(token);
            }
            _context.SaveChanges();
        }
    }
}
