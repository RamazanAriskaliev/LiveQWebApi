using AutoMapper;
using LiveQ.Api.Helpers;
using LiveQ.Api.Models;
using LiveQ.Api.Services;
using LiveQ.Api.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveQ.Api.Controllers
{
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        public AdminController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, ApplicationDbContext appDbContext, IMessageService messageService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _appDbContext = appDbContext;
            _messageService = messageService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegistrationDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = _mapper.Map<AppUser>(model);

            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            await _userManager.AddToRoleAsync(userIdentity, Constants.Strings.JwtClaims.Creator);

            await _appDbContext.SaveChangesAsync();

            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(userIdentity);
            
            var emailConfirmationResult = await _userManager.ConfirmEmailAsync(userIdentity, emailConfirmationToken);
            if (!emailConfirmationResult.Succeeded)
                return Content(emailConfirmationResult.Errors.Select(error => error.Description).Aggregate((allErrors, error) => allErrors += ", " + error));

            return new OkObjectResult("New Creator successfully added!");
        }

    }
}
