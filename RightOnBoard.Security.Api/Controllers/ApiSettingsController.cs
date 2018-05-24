using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RightOnBoard.Security.Api.Models;
using RightOnBoard.Security.Common;

namespace RightOnBoard.Security.Api.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    public class ApiSettingsController : Controller
    {
        private readonly IOptionsSnapshot<ApiSettings> _apiSettingsConfig;

        public ApiSettingsController(IOptionsSnapshot<ApiSettings> apiSettingsConfig)
        {
            _apiSettingsConfig = apiSettingsConfig;
            _apiSettingsConfig.CheckArgumentIsNull(nameof(apiSettingsConfig));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok((_apiSettingsConfig.Value)); //for angular client
        }
    }
}