using Dapper;
using Dapper.Contrib.Extensions;
using System.Threading.Tasks;
using trdb.data.Interface.User;
using trdb.data.Repo.Helpers;
using trdb.entity.Helpers;
using trdb.entity.Users;

namespace trdb.data.Repo.User
{
    public class UserRepository : Connection.dbConnection, IUsers
    {
        public async Task<Users>? Register(Users entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Email", entity.Email);
                param.Add("@Username", entity.Username);
                param.Add("@Password", entity.Password);
                param.Add("@AuthType", entity.AuthType);
                param.Add("@IsActive", entity.IsActive);

                string query = $@"
                DECLARE  @result table(ID Int, Email nvarchar(MAX), Username nvarchar(100), Password nvarchar(MAX), AuthType Int, IsActive bit)
	                    INSERT INTO Users (Email, Username, Password, AuthType, IsActive)
	                        OUTPUT INSERTED.* INTO @result
	                        VALUES ('', 'test', '', 2, 1)
	                    INSERT INTO UserSettingsJunction (UserID, Bio, Picture, Website, IsDMAllowed, IsPublic, IsAdult)
	                        VALUES ((SELECT ID FROM @result), '', NULL, '', 0, 1, 0)
                SELECT *
                FROM @result t
                LEFT JOIN UserSettingsJunction s on s.UserID = t.ID";

                using (var con = GetConnection)
                {
                    var res = await con.QueryAsync<Users, UserSettings, Users>(query, (a, b) =>
                    {
                        a.Settings = b; return a;
                    }, param, splitOn: "ID");
                    return res.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.State = ProcessState.Error;
                return null;
            }
        }

        public async Task<Users>? Login(Users entity)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Email", entity.Email);

                string WhereClause = @" WHERE (t.Email like '%' + @Email + '%')";

                string query = $@"
                SELECT *
                FROM Users t
                LEFT JOIN UserSettingsJunction s on s.UserID = t.ID
                {WhereClause}";

                using (var con = GetConnection)
                {
                    var res = await con.QueryAsync<Users, UserSettings, Users>(query, (a, b) =>
                    {
                        a.Settings = b; return a;
                    }, param, splitOn: "ID");
                    return res.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<bool> CheckEmail(string Email)
        {
            string Query = @"
            SELECT CASE WHEN COUNT(ID) > 0 THEN 1 ELSE 0 END
            FROM Users 
            WHERE Email = @Email";
            DynamicParameters param = new DynamicParameters();
            param.Add("@Email", Email);
            using (var con = GetConnection)
            {
                var res = await con.QueryAsync<bool>(Query, param);
                return res.FirstOrDefault();
            }
        }

        public async Task<bool> CheckUsername(string Username)
        {
            string Query = @"
            SELECT CASE WHEN COUNT(ID) > 0 THEN 1 ELSE 0 END
            FROM Users 
            WHERE Username = @Username";
            DynamicParameters param = new DynamicParameters();
            param.Add("@Username", Username);
            using (var con = GetConnection)
            {
                var res = await con.QueryAsync<bool>(Query, param);
                return res.FirstOrDefault();
            }
        }

        public async Task<ProcessResult>? DeactivateAccount(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string query = $@"
                UPDATE Users
                SET IsActive = 0
                WHERE ID = @ID";

                using (var connection = GetConnection)
                {
                    var res = await connection.QueryAsync<ProcessResult>(query, param);
                    return res.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<ProcessResult> Delete(int ID)
        {
            using (var con = GetConnection)
            {
                ProcessResult pr = new ProcessResult();
                try
                {
                    await con.DeleteAsync(new Users() { ID = ID });
                    pr.ReturnID = 0;
                    pr.Message = "Success";
                    pr.State = ProcessState.Success;
                }
                catch (Exception)
                {
                    pr.ReturnID = 0;
                    pr.Message = "Error";
                    pr.State = ProcessState.Error;
                }
                return pr;
            }
        }

        public async Task<Users>? Get(int? ID, string? Username)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);
                param.Add("@Username", Username);

                string WhereClause = @" WHERE t.ID = @ID OR (t.Username like '%' + @Username + '%')";

                string query = $@"
                SELECT *
                FROM Users t
                LEFT JOIN UserSettingsJunction usj ON usj.UserID = t.ID
                {WhereClause}";

                using (var con = GetConnection)
                {
                    var res = await con.QueryAsync<Users, UserSettings, Users>(query, (user, settings) =>
                    {
                        user.Settings = settings;
                        return user;
                    }, param, splitOn: "ID");
                    return res.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<List<Users>>? GetFollowing(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string query = $@"
                SELECT t.ID, t.Username, usj.Picture, usj.IsPublic
                FROM Users t
                LEFT JOIN UserSettingsJunction usj ON usj.UserID = t.ID
                WHERE t.ID in 
                (select UserID from UserFollowsJunction where FollowerID = @ID)";

                using (var con = GetConnection)
                {
                    var res = await con.QueryAsync<Users, UserSettings, Users>(query, (user, settings) =>
                    {
                        user.Settings = settings;
                        return user;
                    }, param, splitOn: "Picture");
                    return res.ToList();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<List<Users>>? GetFollowers(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string query = $@"
                SELECT t.ID, t.Username, usj.Picture, usj.IsPublic
                FROM Users t
                LEFT JOIN UserSettingsJunction usj ON usj.UserID = t.ID
                WHERE t.ID in 
                (select FollowerID from UserFollowsJunction where UserID = @ID)";

                using (var con = GetConnection)
                {
                    var res = await con.QueryAsync<Users, UserSettings, Users>(query, (user, settings) =>
                    {
                        user.Settings = settings;
                        return user;
                    }, param, splitOn: "Picture");
                    return res.ToList();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<List<Users>>? GetBlocklist(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string query = $@"
                SELECT t.ID, t.Username, usj.Picture, usj.IsPublic
                FROM Users t
                LEFT JOIN UserSettingsJunction usj ON usj.UserID = t.ID
                WHERE t.ID in 
                (select UserID from UserBlocklistJunction where BlockerID = @ID)";

                using (var con = GetConnection)
                {
                    var res = await con.QueryAsync<Users, UserSettings, Users>(query, (user, settings) =>
                    {
                        user.Settings = settings;
                        return user;
                    }, param, splitOn: "Picture");
                    return res.ToList();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<ProcessResult> Update(Users entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    bool res = await con.UpdateAsync(entity);
                    if (res == true)
                    {
                        result.ReturnID = entity.ID;
                        result.Message = "User updated successfully.";
                        result.State = ProcessState.Success;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.State = ProcessState.Error;
            }
            return result;
        }

        public async Task<ProcessResult>? UpdateEmail(int ID, string Email)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);
                param.Add("@Mail", Email);

                string query = $@"
                UPDATE Users
                SET Email = @Email
                WHERE ID = @ID";

                using (var connection = GetConnection)
                {
                    var res = await connection.QueryAsync<ProcessResult>(query, param);
                    return res.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<ProcessResult>? UpdatePassword(int ID, string Password)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);
                param.Add("@Password", Password);

                string query = $@"
                UPDATE Users
                SET Password = @Password
                WHERE ID = @ID";

                using (var connection = GetConnection)
                {
                    var res = await connection.QueryAsync<ProcessResult>(query, param);
                    return res.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<ProcessResult>? UpdateUsername(int ID, string Username)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);
                param.Add("@Username", Username);

                string query = $@"
                UPDATE Users
                SET Username = @Username
                WHERE ID = @ID";

                using (var connection = GetConnection)
                {
                    var res = await connection.QueryAsync<ProcessResult>(query, param);
                    return res.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }
    }
}
