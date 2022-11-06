using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using trdb.data.Interface.User;
using trdb.data.Repo.User;
using trdb.entity.Helpers;
using trdb.entity.Returns;
using trdb.entity.UserMovies;

namespace trdb.business.Users
{
    public class UserManager : IUsers
    {
        private readonly IUsers _repo;
        public UserManager()
        {
            _repo = new UserRepository();
        }

        private string generateToken(entity.Users.Users user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("id", user.ID.ToString()),
                    new Claim("authType", user.AuthType.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<entity.Users.Users>? Register(entity.Users.Users entity)
        {
            if (entity.Username == null || entity.Email == null || entity.Password == null)
            {
                throw new Exception("User information missing");
            }

            bool userExists = await CheckEmail(entity.Email, null);
            if (userExists)
            {
                throw new Exception("Email address already registered");
            }

            bool usernameExists = await CheckUsername(entity.Username, null);
            if (usernameExists)
            {
                throw new Exception("Username already exists");
            }

            var salt = BCrypt.Net.BCrypt.GenerateSalt(10);
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(entity.Password, salt);
            entity.Password = hashedPassword;
            entity.AuthType = 1;
            entity.IsActive = true;

            var result = await _repo.Register(entity);
            if (result != null)
            {
                result.AuthType = entity.AuthType;
                var user = new entity.Users.Users();
                user.ID = entity.ID;
                user.Username = entity.Username;
                user.Email = entity.Email;
                user.Token = generateToken(result);
                user.Settings = result.Settings;
                return user;
            }
            throw new Exception("Server error.");
        }

        public async Task<entity.Users.Users>? Login(entity.Users.Users entity)
        {
            if (entity.Email == null || entity.Password == null)
            {
                throw new Exception("User information missing");
            }

            var result = await _repo.Login(entity);

            if (result != null && BCrypt.Net.BCrypt.Verify(entity.Password, result.Password))
            {
                var user = new entity.Users.Users();
                user.ID = result.ID;
                user.Username = result.Username;
                user.Email = result.Email;
                user.Token = generateToken(result);
                user.Settings = result.Settings;
                return user;
            }

            throw new Exception("Invalid credentials");
        }

        public async Task<bool> CheckEmail(string Email, int? UserID)
        {
            return await _repo.CheckEmail(Email, UserID);
        }

        public async Task<bool> CheckUsername(string Username, int? UserID)
        {
            return await _repo.CheckUsername(Username, UserID);
        }

        public async Task<ProcessResult>? DeactivateAccount(int ID)
        {
            return await _repo.DeactivateAccount(ID);
        }

        public async Task<ProcessResult> Delete(int ID)
        {
            return await _repo.Delete(ID);
        }


        public async Task<entity.Users.Users>? Get(int? ID, string? Username)
        {
            return await _repo.Get(ID, Username);
        }

        public async Task<List<entity.Users.Users>>? GetFollowers(int ID)
        {
            return await _repo.GetFollowers(ID);
        }

        public async Task<List<entity.Users.Users>>? GetFollowing(int ID)
        {
            return await _repo.GetFollowing(ID);
        }

        public async Task<List<entity.Users.Users>>? GetBlocklist(int ID)
        {
            return await _repo.GetBlocklist(ID);
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

        public async Task<bool> Follow(int targetID, int userID)
        {
            return await _repo.Follow(targetID, userID);
        }

        public async Task<bool> Block(int targetID, int userID)
        {
            return await _repo.Block(targetID, userID);
        }

        public async Task<bool> ToggleDMs(int userID)
        {
            return await _repo.ToggleDMs(userID);
        }

        public async Task<bool> TogglePrivacy(int userID)
        {
            return await _repo.TogglePrivacy(userID);
        }

        public async Task<bool> ToggleAdultContent(int userID)
        {
            return await _repo.ToggleAdultContent(userID);
        }

        public async Task<SettingsReturn> UpdatePersonalSettings(SettingsReturn entity, int userID)
        {
            return await _repo.UpdatePersonalSettings(entity, userID);
        }

        public async Task<string> UpdateAvatar(string path, int userID)
        {
            return await _repo.UpdateAvatar(path, userID);
        }

        public async Task<List<UserFavoriteMovies>> ManageFavoriteMovies(List<UserFavoriteMovies> entity, int userID)
        {
            return await _repo.ManageFavoriteMovies(entity, userID);
        }
    }
}
