using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Identity.Infrastructure
{
    public abstract class CookieWriter
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected CookieWriter(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        protected HttpContext Context => _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext is not available outside of an HTTP request.");

        protected CookieOptions BuildDefaultOptions()
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = _environment.IsDevelopment() ? SameSiteMode.None : SameSiteMode.Lax,
                Path = "/"
            };
        }

        protected void AppendCookie(string name, string value, DateTimeOffset expiresAt)
        {
            var options = BuildDefaultOptions();
            options.Expires = expiresAt;
            Context.Response.Cookies.Append(name, value, options);
        }

        protected void DeleteCookie(string name)
        {
            var options = BuildDefaultOptions();
            Context.Response.Cookies.Delete(name, options);
        }
    }
}
