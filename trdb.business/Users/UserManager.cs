using trdb.data.Interface.User;
using trdb.data.Repo.User;
using trdb.entity.Helpers;

namespace trdb.business.Users
{
    public class UserManager : IUsers
    {
        private readonly IUsers _repo;
        public UserManager()
        {
            _repo = new UserRepository();
        }

        public async Task<ProcessResult> Register(entity.Users.Users entity)
        {
            return await _repo.Register(entity);
        }

        public async Task<bool> CheckEmail(string Email)
        {
            return await _repo.CheckEmail(Email);
        }

        public async Task<bool> CheckUsername(string Username)
        {
            return await _repo.CheckUsername(Username);
        }

        public async Task<ProcessResult>? DeactivateAccount(int ID)
        {
            return await _repo.DeactivateAccount(ID);
        }

        public async Task<ProcessResult> Delete(int ID)
        {
            return await _repo.Delete(ID);
        }

        public async Task<entity.Users.Users>? Get(int ID)
        {
            return await _repo.Get(ID);
        }

        public async Task<entity.Users.Users>? Login(string Email)
        {
            return await _repo.Login(Email);
        }

        public async Task<ProcessResult>? Update(entity.Users.Users entity)
        {
            return await _repo.Update(entity);
        }

        public async Task<ProcessResult>? UpdateEmail(int ID, string Email)
        {
            return await _repo.UpdateEmail(ID, Email);
        }

        public async Task<ProcessResult>? UpdatePassword(int ID, string Password)
        {
            return await _repo.UpdatePassword(ID, Password);
        }

        public async Task<ProcessResult>? UpdateUsername(int ID, string Username)
        {
            return await _repo.UpdateUsername(ID, Username);
        }
    }
}
