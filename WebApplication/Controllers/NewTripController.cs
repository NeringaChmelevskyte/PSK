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
using Microsoft.AspNetCore.Mvc.Filters;

namespace Application.Controllers
{
    [Produces("application/json")]
    public class NewTripController : Controller
    {
        private IUserService _us;
        private User user;

        public NewTripController(IUserService us)
        {
            _us = us;
        }
        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
            user = _us.GetUserFromRequest(Request);
            if (user == null)
            {
                ViewBag.Name = "";
                ViewBag.Role = null;
            }

            else
            {
                ViewBag.Name = user.Name + " " + user.Surname;
                ViewBag.Role = user.Role;
            }
        }

        public IActionResult Index()
        {
            if (user.Role == Roles.Organizer || user.Role == Roles.Admin)
            {
                var users = _us.GetAllUsers();
                return View(users);
            }
            else return Unauthorized();
        }

        public IActionResult Search(string userList)
        {
            List<string> stringUseriai = userList == null ? new List<string>() : userList.Split(", ").ToList();
            var useriai = _us.GetUserListFromStringList(stringUseriai);

            //Čia turime Userius kuriems norima kurti kelionę. Reikia patikrinti laisvą laiką ir pildyti toliau formą

            if (user.Role == Roles.Organizer || user.Role == Roles.Admin)
            {
                return View(useriai);
            }
            else return Unauthorized();
        }
    }
}