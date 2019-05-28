using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.IServices
{
    public interface ITripService
    {
        void ApproveTripForUser(int tripId, int userId);
        void DeclineTripForUser(int tripId, int userId);
    }
}
