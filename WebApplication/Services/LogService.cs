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
    public class LogService : ILogService
    {
        protected readonly ApplicationDbContext _context;
        public LogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddLogMessage(string message)
        {
            Log entity = new Log
            {
                Time = DateTime.Now,
                Text = message
            };
            _context.Add(entity);
            _context.SaveChanges();
        }
    }
}
