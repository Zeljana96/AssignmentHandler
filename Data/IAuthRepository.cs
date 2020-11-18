using System.Threading.Tasks;
using Tasks_Handler.Models;

namespace Tasks_Handler.Data
{
    public interface IAuthRepository
    {
         Task<ServiceResponse<int>> Register (User user, string password);
         Task<ServiceResponse<string>> Login (string email, string password);
         Task<bool> UserExists (string email);

         Task<ServiceResponse<string>> Logout();
    }
}