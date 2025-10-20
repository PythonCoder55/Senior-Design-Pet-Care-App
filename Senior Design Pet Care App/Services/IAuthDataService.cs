using System.Security.Claims;
using System.Threading.Tasks;
using Senior_Design_Pet_Care_App.Entities;

namespace Senior_Design_Pet_Care_App.Services
{
    public interface IAuthDataService
    {
        Task<ServiceResponse<ClaimsPrincipal>> LoginAsync(string email, string password);
        Task<ServiceResponse<bool>> RegisterAsync(string email, string password, string? role = null);
    }
}