using AutoMapper;
using LiveQ.Api.ViewModels;
using LiveQ.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LiveQ.Api.Helpers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using LiveQ.Api.Services;
using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace LiveQ.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, ApplicationDbContext appDbContext, IMessageService messageService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _appDbContext = appDbContext;
            _messageService = messageService;
        }
        // POST api/accounts
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody]RegistrationDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = _mapper.Map<AppUser>(model);

            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            await _userManager.AddToRoleAsync(userIdentity, Constants.Strings.JwtClaims.Subscriber);
            await _userManager.AddClaimAsync(userIdentity, new Claim(ClaimTypes.Role, Constants.Strings.JwtClaims.Subscriber));
            await _appDbContext.SaveChangesAsync();

            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(userIdentity);
            var tokenVerificationUrl = Url.Action("VerifyEmail", "Accounts", new { id = userIdentity.Id, token = emailConfirmationToken }, Request.Scheme);

            await _messageService.Send(userIdentity.Email, "Verify your email", $"Click <a href=\"{tokenVerificationUrl}\">here</a> to verify your email");


            return new OkObjectResult("Check your email for a verification link");
        }

        [AllowAnonymous]
        [HttpGet("verify")]
        public async Task<IActionResult> VerifyEmail(string id, string token)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new InvalidOperationException();

            var emailConfirmationResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!emailConfirmationResult.Succeeded)
                return Content(emailConfirmationResult.Errors.Select(error => error.Description).Aggregate((allErrors, error) => allErrors += ", " + error));
                
            return new OkObjectResult("Email confirmed, you can now log in,");
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody]UpdateAccountDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            String userName = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userIdentity = await _userManager.FindByNameAsync(userName);
            if (userIdentity == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Login please", ModelState));
            }
            userIdentity.FirstName = model.FirstName;
            userIdentity.LastName = model.LastName;
            var result =  await _userManager.UpdateAsync(userIdentity);
            if (result.Succeeded)
            {
                return new OkObjectResult("Account seccessfully updated!");
            }
            return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
        }

        [HttpPost("forgot")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            AppUser user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest(Errors.AddErrorToModelState("user_not_found", "Invalid email address", ModelState));
            }
            var emailConfirmationToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var tokenVerificationUrl = Url.Action("ResetPassword", "Accounts", new { id = user.Id, token = emailConfirmationToken }, Request.Scheme);

            await _messageService.Send(user.Email, "Reset Password", $"Please reset your password by using this: \"{ emailConfirmationToken}\"");
            return Ok("Check your email!");
        }

        [HttpPost("reset")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            AppUser user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest(Errors.AddErrorToModelState("user_not_found", "Invalid email address", ModelState));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(Errors.AddErrorToModelState("Pasword_reset_fail", "Fail", ModelState));
            }
            return Ok("Now you can logIn with new password");
        }


    }
}
