using trdb.entity.Helpers;
using trdb.entity.Users;

namespace trdb.data.Interface.User
{
    public interface IUsers
    {
        Task<bool> CheckEmail(string Email);
        Task<bool> CheckUsername(string Username);
        Task<Users>? Login(Users entity);
        Task<Users>? Register(Users entity);
        Task<ProcessResult> Update(Users entity);
        Task<ProcessResult> Delete(int ID);
        Task<Users>? Get(int ID);
        Task<ProcessResult>? UpdateUsername(int ID, string Username);
        Task<ProcessResult>? UpdatePassword(int ID, string Password);
        Task<ProcessResult>? UpdateEmail(int ID, string Email);
        Task<ProcessResult>? DeactivateAccount(int ID);
    }
}
