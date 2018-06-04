using LiveQ.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LiveQ.Api.Services
{
    public interface IUserService
    {
        Task<IList<Claim>> GetClaims(AppUser user);
    }
}
