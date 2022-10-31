using trdb.entity.Helpers;
using trdb.entity.Users;

namespace trdb.data.Interface.User
{
    public interface IUsers
    {
        Task<bool> CheckEmail(string Email, int? UserID);
        Task<bool> CheckUsername(string Username, int? UserID);
        Task<Users>? Login(Users entity);
        Task<Users>? Register(Users entity);
        Task<ProcessResult> Update(Users entity);
        Task<ProcessResult> Delete(int ID);
        Task<Users>? Get(int? ID, string? Username);
        Task<bool> Follow(int targetID, int userID);
        Task<bool> Block(int targetID, int userID);
        Task<SettingsReturn> UpdatePersonalSettings(SettingsReturn entity, int userID);
        Task<string> UpdateAvatar(string path, int userID);
        Task<bool> ToggleDMs(int userID);
        Task<bool> TogglePrivacy(int userID);
        Task<bool> ToggleAdultContent(int userID);
        Task<List<Users>>? GetFollowing(int ID);
        Task<List<Users>>? GetFollowers(int ID);
        Task<List<Users>>? GetBlocklist(int ID);
        Task<ProcessResult>? UpdateUsername(int ID, string Username);
        Task<ProcessResult>? UpdatePassword(int ID, string Password);
        Task<ProcessResult>? UpdateEmail(int ID, string Email);
        Task<ProcessResult>? DeactivateAccount(int ID);
    }
}
