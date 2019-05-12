using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.IServices;
using Application.Entities;

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
            if(user != null)
                return RedirectToAction("AllUsers", "Users");
                else return Unauthorized();

        }

        [HttpPost]
        public ActionResult AddUser(User value)
        {
            _us.AddUser(value);
            return RedirectToAction("AllUsers", "Users");
        }

        //[HttpPost]
        //public void Post(int id)
        //{
        //    _us.RemoveUser(id);
        //}

        public IActionResult AddUserView()
        {
            return View();
        }

        public IActionResult LoginView()
        {
            return View();
        }
    }
}