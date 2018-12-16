using System.Threading.Tasks;

namespace DotCommon.AspNetCore.Mvc.Demo.Services
{
    [RemoteService]
    public class UserService : IUserService
    {
        public async Task<string> GetUserName(int id)
        {

            return await Task.FromResult("zhangsan");
        }
    }
}
