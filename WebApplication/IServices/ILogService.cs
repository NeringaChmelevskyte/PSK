using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.IServices
{
    public interface ILogService
    {
        void AddLogMessage(string message);
    }
}
