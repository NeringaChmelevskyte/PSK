using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.IServices;
using Application.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Application.Controllers
{
    [Produces("application/json")]
    public class UsersController : Controller
    {
        private IUserService _us;

        public UsersController(IUserService us)
        {
            _us = us;
        }

        public IActionResult AllUsers()
        {
            var users = Get();
            return View(users);
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
            return _us.GetAllUsers();
        }

        [HttpPost]
        public ActionResult Login(User value)
        {
            User user = _us.LoginUser(value.Email, value.Password);
            if (user != null)
            {
                //var claim = new Claim(JwtRegisteredClaimNames.Sub, user.Email);
                var signinKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes("BMW 530 e60 160kw"));

                int expiryInMinutes = 600;

                var token = new JwtSecurityToken(
                  issuer: "Musu app",
                  audience: "Musu app",
                  expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                  signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                );
                var stringToken = new JwtSecurityTokenHandler().WriteToken(token);

                _us.AddToken(user, stringToken);

                CookieOptions options = new CookieOptions();
                options.Expires = token.ValidTo;
                Response.Cookies.Append("token", stringToken, options);

                return RedirectToAction("AllUsers", "Users");
            }
            else
            {
                return Unauthorized();
            }

        }

        [HttpPost]
        public ActionResult AddUser(User value)
        {
            _us.AddUser(value);
            return RedirectToAction("AllUsers", "Users");
        }

        [HttpPost]
        public ActionResult RemoveUser(int id)
        {
            _us.RemoveUser(id);
            return RedirectToAction("AllUsers", "Users");
        }

        [HttpPost]
        public ActionResult EditUser(User user)
        {
            _us.UpdateUser(user);
            return RedirectToAction("AllUsers", "Users");
        }
        
        public IActionResult AddUserView()
        {
            var enumData = from Roles r in Enum.GetValues(typeof(Roles))
                           select new
                           {
                               ID = (int)r,
                               Title = r.ToString()
                           };
            ViewBag.EnumList = new SelectList(enumData, "ID", "Title");
            return View();
        }

        public IActionResult DeleteView(int? id)
        {
            var user= Get().Where(x => x.Id == id);
            return View(user.SingleOrDefault());
        }

        public IActionResult EditUserView(User user)
        {
            var enumData = from Roles r in Enum.GetValues(typeof(Roles))
                           select new
                           {
                               ID = (int)r,
                               Title = r.ToString()
                           };
            ViewBag.EnumList = new SelectList(enumData, "ID", "Title");
            return View(user);
        }
        public IActionResult Logout()
        {
            var user = _us.GetUserFromRequest(Request);
            _us.DeleteToken(user);
            return RedirectToAction("LoginView");
        }
        public IActionResult LoginView()
        {
            return View();
        }
    }
}