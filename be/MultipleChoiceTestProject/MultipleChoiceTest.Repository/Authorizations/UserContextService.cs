using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MultipleChoiceTest.Repository.Authorizations
{
    public interface IUserContextService
    {
        string GetCurrentUsername();
    }

    public class UserContextService : IUserContextService
    {
        public readonly IHttpContextAccessor _httpContextAccessor;
        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUsername()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
}
