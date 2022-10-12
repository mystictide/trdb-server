using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

            bool userExists = await CheckEmail(entity.Email);
            if (userExists)
            {
                throw new Exception("Email address already registered");
            }

            bool usernameExists = await CheckUsername(entity.Username);
            if (usernameExists)
            {
                throw new Exception("Username already exists");
            }

            var salt = BCrypt.Net.BCrypt.GenerateSalt(10);
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(entity.Password, salt);
            entity.Password = hashedPassword;
            entity.AuthType = 3;
            entity.IsActive = true;

            var result = await _repo.Register(entity);
            if (result != null)
            {
                result.AuthType = entity.AuthType;
                var user = new entity.Users.Users();
                user.Username = entity.Username;
                user.Token = generateToken(result);
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
                user.Username = result.Username;
                user.Token = generateToken(result);
                return user;
            }

            throw new Exception("Invalid credentials");
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
