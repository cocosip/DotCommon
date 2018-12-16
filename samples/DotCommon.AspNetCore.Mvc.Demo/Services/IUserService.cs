using System.Threading.Tasks;

namespace DotCommon.AspNetCore.Mvc.Demo.Services
{

    public interface IUserService : IRemoteService
    {
        Task<string> GetUserName(int id);
    }
}
