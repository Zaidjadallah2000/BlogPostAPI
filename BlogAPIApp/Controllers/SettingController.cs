using Blogpp.Models;
using Blogpp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPIApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly ISettingService setingService;

        public SettingController(ISettingService _setingService)
        {
            setingService = _setingService;
        }
        [HttpPut]
        public async Task<string> updateAsync(SettingDTO settingDTO)
        {
            var result = await setingService.UpdateProfile(settingDTO);
            if (result == "User not found")
            {
               return ("User Not Found");
            }
            else if (result == "currentPassError")
            {
                return ("Current Password Incorrect");
            }
            else
            {
                return "success";
            }
        }
    }
}
