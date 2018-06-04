using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LiveQ.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace LiveQ.Api.Services
{
    public class UserService : IUserService
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private IPasswordHasher<AppUser> _passwordHasher;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IPasswordHasher<AppUser> hasher)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _passwordHasher = hasher;
        }
        public async Task<IList<Claim>> GetClaims(AppUser user)
        {
            return await _userManager.GetClaimsAsync(user);
        }
    }
}
