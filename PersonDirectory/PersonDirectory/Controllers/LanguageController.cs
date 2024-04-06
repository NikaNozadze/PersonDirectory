using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace PersonDirectory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LanguageController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        [HttpGet("change-language-en")]
        public IActionResult ChangeCultureToEN()
        {
            try
            {
                if (!string.IsNullOrEmpty("en-US"))
                {
                    _httpContextAccessor?.HttpContext?.Response.Cookies.Append(
                        CookieRequestCultureProvider.DefaultCookieName,
                        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("en-US")),
                        new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                    );
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("change-language-ka")]
        public IActionResult ChangeCultureToGE()
        {
            try
            {
                if (!string.IsNullOrEmpty("ka-GE"))
                {
                    _httpContextAccessor?.HttpContext?.Response.Cookies.Append(
                        CookieRequestCultureProvider.DefaultCookieName,
                        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("ka-GE")),
                        new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                    );
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
