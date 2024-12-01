using Blogpp.Models;
using Blogpp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogAPIApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService _accountService)
        {
            accountService = _accountService;
        }
        [HttpPost]
        [Route("checkLogin")]
        public async Task<IActionResult> Login(SignInDTO signIn)
        {
          var result = await accountService.checkLogin(signIn);
            if (result == "EmailNotFound")
            {
                return Unauthorized();
            }else if(result == "Success")
            {
                List<Claim> authClaim = new List<Claim>();
                authClaim.Add(new Claim(ClaimTypes.Name,signIn.Email));
                authClaim.Add(new Claim("uniqeValue",Guid.NewGuid().ToString()));
                var UserRole = await accountService.RoleUser(signIn.Email);
                foreach (var role in UserRole)
                {
                    authClaim.Add(new Claim(ClaimTypes.Role, role));
                }
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsMyLongSecurityKey1234567890"));

                var token = new JwtSecurityToken(
                issuer: "http://localhost",
                audience: "User",
                expires: DateTime.Now.AddDays(15),
                claims: authClaim,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new
                {
                    tokenValue = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            else
            {
                return Unauthorized();

            }
        }


        [HttpPost]
        [Route("AddUser")]
        public async Task insert(SignUpDTO signUpDTO)
        {
           await accountService.insert(signUpDTO);
        }
        [HttpPost]
        [Route("AddRole")]
        public async Task inserRole(RoleDTO roleDTO) {
            await accountService.AddRole(roleDTO);
        }
        [HttpGet]
        [Route("GelAllRoles")]
        public List<RoleDTO> getAll()
        {
           return accountService.GetAllRoles();
        }
    }
}
