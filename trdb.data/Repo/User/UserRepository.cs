using Dapper;
using Dapper.Contrib.Extensions;
using trdb.data.Interface.User;
using trdb.data.Repo.Helpers;
using trdb.entity.Helpers;
using trdb.entity.Returns;
using trdb.entity.Users;
using trdb.entity.Users.Settings;

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
	                    INSERT INTO UserSettingsJunction (UserID, Bio, Picture, Website, IsDMAllowed, IsWatchlistPublic, IsPublic, IsAdult)
	                        VALUES ((SELECT ID FROM @result), '', NULL, '', 1, 1, 1, 0)
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

                string favFilmQuery = $@"
                SELECT t.ID, t.FilmID, t.SortOrder, m.Title, m.Backdrop_URL, m.Poster_URL
                FROM UserFavoriteFilmsJunction t
                LEFT JOIN Films m ON m.TMDB_ID = t.FilmID
                WHERE UserID = @ID
                ORDER BY t.SortOrder ASC";
                string favActorsQuery = $@"
                SELECT t.ID, t.PersonID, t.SortOrder, m.Name, m.Photo_URL
                FROM UserFavoritePeopleJunction t
                LEFT JOIN People m ON m.TMDB_ID = t.PersonID
                WHERE UserID = @ID AND IsActor = 1
                ORDER BY t.SortOrder ASC";
                string favDirectorsQuery = $@"
                SELECT t.ID, t.PersonID, t.SortOrder, m.Name, m.Photo_URL
                FROM UserFavoritePeopleJunction t
                LEFT JOIN People m ON m.TMDB_ID = t.PersonID
                WHERE UserID = @ID AND IsActor = 0
                ORDER BY t.SortOrder ASC";

                using (var con = GetConnection)
                {
                    var res = await con.QueryAsync<Users, UserSettings, Users>(query, (a, b) =>
                    {
                        a.Settings = b; return a;
                    }, param, splitOn: "ID");
                    param.Add("@ID", res.FirstOrDefault().ID);
                    var favFilms = await con.QueryAsync<UserFavoriteFilms>(favFilmQuery, param);
                    var favActors = await con.QueryAsync<UserFavoritePeople>(favActorsQuery, param);
                    var favDirectors = await con.QueryAsync<UserFavoritePeople>(favDirectorsQuery, param);
                    if (favFilms != null)
                    {
                        res.FirstOrDefault().Settings.FavoriteFilms = favFilms.ToList();
                    }
                    if (favActors != null)
                    {
                        res.FirstOrDefault().Settings.FavoriteActors = favActors.ToList();
                    }
                    if (favDirectors != null)
                    {
                        res.FirstOrDefault().Settings.FavoriteDirectors = favDirectors.ToList();
                    }
                    return res.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<bool> CheckEmail(string Email, int? UserID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@UserID", UserID);
            param.Add("@Email", Email);

            string Query;
            if (UserID.HasValue)
            {
                Query = @"
                SELECT CASE WHEN COUNT(ID) > 0 THEN 1 ELSE 0 END
                FROM Users 
                WHERE Email = @Email AND NOT (ID = @UserID)";
            }
            else
            {
                Query = @"
                SELECT CASE WHEN COUNT(ID) > 0 THEN 1 ELSE 0 END
                FROM Users 
                WHERE Email = @Email";
            }

            using (var con = GetConnection)
            {
                var res = await con.QueryAsync<bool>(Query, param);
                return res.FirstOrDefault();
            }
        }

        public async Task<bool> CheckUsername(string Username, int? UserID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@UserID", UserID);
            param.Add("@Username", Username);

            string Query;
            if (UserID.HasValue)
            {
                Query = @"
                SELECT CASE WHEN COUNT(ID) > 0 THEN 1 ELSE 0 END
                FROM Users 
                WHERE Username = @Username AND NOT (ID = @UserID)";
            }
            else
            {
                Query = @"
                SELECT CASE WHEN COUNT(ID) > 0 THEN 1 ELSE 0 END
                FROM Users 
                WHERE Username = @Username";
            }

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

                string favFilmQuery = $@"
                SELECT t.ID, t.FilmID, t.SortOrder, m.Title, m.Backdrop_URL, m.Poster_URL
                FROM UserFavoriteFilmsJunction t
                LEFT JOIN Films m ON m.TMDB_ID = t.FilmID
                WHERE UserID = @ID
                ORDER BY t.SortOrder ASC";
                string favActorsQuery = $@"
                SELECT t.ID, t.PersonID, t.SortOrder, m.Name, m.Photo_URL
                FROM UserFavoritePeopleJunction t
                LEFT JOIN People m ON m.TMDB_ID = t.PersonID
                WHERE UserID = @ID AND IsActor = 1
                ORDER BY t.SortOrder ASC";
                string favDirectorsQuery = $@"
                SELECT t.ID, t.PersonID, t.SortOrder, m.Name, m.Photo_URL
                FROM UserFavoritePeopleJunction t
                LEFT JOIN People m ON m.TMDB_ID = t.PersonID
                WHERE UserID = @ID AND IsActor = 0
                ORDER BY t.SortOrder ASC";

                using (var con = GetConnection)
                {
                    var res = await con.QueryAsync<Users, UserSettings, Users>(query, (user, settings) =>
                    {
                        user.Settings = settings;
                        return user;
                    }, param, splitOn: "ID");
                    param.Add("@ID", res.FirstOrDefault().ID);
                    var favFilms = await con.QueryAsync<UserFavoriteFilms>(favFilmQuery, param);
                    var favActors = await con.QueryAsync<UserFavoritePeople>(favActorsQuery, param);
                    var favDirectors = await con.QueryAsync<UserFavoritePeople>(favDirectorsQuery, param);
                    if (favFilms != null)
                    {
                        res.FirstOrDefault().Settings.FavoriteFilms = favFilms.ToList();
                    }
                    if (favActors != null)
                    {
                        res.FirstOrDefault().Settings.FavoriteActors = favActors.ToList();
                    }
                    if (favDirectors != null)
                    {
                        res.FirstOrDefault().Settings.FavoriteDirectors = favDirectors.ToList();
                    }
                    return res.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<bool> Follow(int TargetID, int UserID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@TargetID", TargetID);
            param.Add("@UserID", UserID);

            string Query = @"
            IF EXISTS(SELECT * from UserFollowsJunction where UserID = @TargetID AND FollowerID = @UserID)        
            BEGIN            
	            Delete from UserFollowsJunction WHERE UserID = @TargetID AND FollowerID = @UserID
            END                    
            ELSE            
            BEGIN  
	            INSERT INTO UserFollowsJunction (UserID, FollowerID, Date)
	            VALUES (@TargetID, @UserID, GETDATE())
            END
            SELECT CASE WHEN EXISTS (
                SELECT *
                FROM UserFollowsJunction
                WHERE UserID = @TargetID AND FollowerID = @UserID
            )
            THEN CAST(1 AS BIT)
            ELSE CAST(0 AS BIT) END";

            using (var con = GetConnection)
            {
                var res = await con.QueryAsync<bool>(Query, param);
                return res.FirstOrDefault();
            }
        }

        public async Task<bool> Block(int TargetID, int UserID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@TargetID", TargetID);
            param.Add("@UserID", UserID);

            string Query = @"
            IF EXISTS(SELECT * from UserBlocklistJunction where UserID = @TargetID AND BlockerID = @UserID)        
            BEGIN            
	            Delete from UserBlocklistJunction WHERE UserID = @TargetID AND BlockerID = @UserID
            END                    
            ELSE            
            BEGIN  
	            INSERT INTO UserBlocklistJunction (UserID, BlockerID)
	            VALUES (@TargetID, @UserID)
            END
            SELECT CASE WHEN EXISTS (
                SELECT *
                FROM UserBlocklistJunction
                WHERE UserID = @TargetID AND BlockerID = @UserID
            )
            THEN CAST(1 AS BIT)
            ELSE CAST(0 AS BIT) END";

            using (var con = GetConnection)
            {
                var res = await con.QueryAsync<bool>(Query, param);
                return res.FirstOrDefault();
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

        public async Task<bool> ToggleDMs(int userID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@UserID", userID);

            string query = $@"
                UPDATE UserSettingsJunction
                SET IsDMAllowed = ~IsDMAllowed
                WHERE UserID = @UserID
                Select IsDMAllowed from UserSettingsJunction where UserID = @UserID";

            using (var con = GetConnection)
            {
                var res = await con.QueryFirstAsync<bool>(query, param);
                return res;
            }
        }

        public async Task<bool> ToggleWatchlist(int userID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@UserID", userID);

            string query = $@"
                UPDATE UserSettingsJunction
                SET IsWatchlistPublic = ~IsWatchlistPublic
                WHERE UserID = @UserID
                Select IsWatchlistPublic from UserSettingsJunction where UserID = @UserID";

            using (var con = GetConnection)
            {
                var res = await con.QueryFirstAsync<bool>(query, param);
                return res;
            }
        }

        public async Task<bool> TogglePrivacy(int userID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@UserID", userID);

            string query = $@"
                UPDATE UserSettingsJunction
                SET IsPublic = ~IsPublic
                WHERE UserID = @UserID
                Select IsPublic from UserSettingsJunction where UserID = @UserID";

            using (var con = GetConnection)
            {
                var res = await con.QueryFirstAsync<bool>(query, param);
                return res;
            }
        }

        public async Task<bool> ToggleAdultContent(int userID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@UserID", userID);

            string query = $@"
                UPDATE UserSettingsJunction
                SET IsAdult = ~IsAdult
                WHERE UserID = @UserID
                Select IsAdult from UserSettingsJunction where UserID = @UserID";

            using (var con = GetConnection)
            {
                var res = await con.QueryFirstAsync<bool>(query, param);
                return res;
            }
        }

        public async Task<SettingsReturn> UpdatePersonalSettings(SettingsReturn entity, int userID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@UserID", userID);
            param.Add("@Bio", entity.Bio);
            param.Add("@Email", entity.Email);
            param.Add("@Username", entity.Username);
            param.Add("@Website", entity.Website);

            string query = $@"
                UPDATE UserSettingsJunction
                SET  Bio = IsNull(@Bio, Bio),
	                 Website = IsNull(@Website, Website)
                WHERE UserID = @UserID
                IF NOT EXISTS(SELECT * from Users WHERE Email = @Email)
                UPDATE Users
	                SET  Email = IsNull(@Email, Email)
                WHERE ID = @UserID
                IF NOT EXISTS(SELECT * from Users WHERE Username = @Username)
                UPDATE Users
	                SET  Username = IsNull(@Username, Username)
                WHERE ID = @UserID

                Select Bio, Website  
                ,(select Email from Users where ID = @UserID) as Email
                ,(select Username from Users where ID = @UserID) as Username
                from UserSettingsJunction
                Where UserID = @UserID";

            using (var con = GetConnection)
            {
                var res = await con.QueryFirstAsync<SettingsReturn>(query, param);
                return res;
            }
        }

        public async Task<string> UpdateAvatar(string path, int userID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@UserID", userID);
            param.Add("@path", path);

            string query = $@"
                UPDATE UserSettingsJunction
                SET  Picture = IsNull(@path, Picture)
                WHERE UserID = @UserID

                Select Picture
                from UserSettingsJunction
                Where UserID = @UserID";

            using (var con = GetConnection)
            {
                var res = await con.QueryFirstAsync<string>(query, param);
                return res;
            }
        }

        public async Task<List<UserFavoriteFilms>> ManageFavoriteFilms(List<UserFavoriteFilms> entity, int userID)
        {
            var result = new List<UserFavoriteFilms>();
            try
            {
                using (var con = GetConnection)
                {
                    foreach (var item in entity)
                    {
                        #region params
                        DynamicParameters param = new DynamicParameters();
                        param.Add("@ID", item.ID);
                        param.Add("@UserID", userID);
                        param.Add("@FilmID", item.FilmID);
                        param.Add("@SortOrder", item.SortOrder);
                        #endregion

                        string query = $@"
                        DELETE FROM UserFavoriteFilmsJunction WHERE UserID = @UserID 
                        INSERT INTO UserFavoriteFilmsJunction (UserID, FilmID, SortOrder)
                        VALUES (@UserID, @FilmID, @SortOrder)
                        SELECT t.ID, t.FilmID, t.SortOrder, m.Title, m.Backdrop_URL, m.Poster_URL
                        FROM UserFavoriteFilmsJunction t
                        LEFT JOIN Films m ON m.TMDB_ID = t.FilmID
                        WHERE UserID = @UserID
                        ORDER BY t.SortOrder ASC";

                        var res = await con.QueryFirstOrDefaultAsync<UserFavoriteFilms>(query, param);
                        result.Add(res);
                    }
                    con.Dispose();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
            result = result.OrderBy(m => m.ID).ToList();
            return result;
        }
        public async Task<List<UserFavoritePeople>> ManageFavoriteActors(List<UserFavoritePeople> entity, int userID)
        {
            var result = new List<UserFavoritePeople>();
            try
            {
                using (var con = GetConnection)
                {
                    foreach (var item in entity)
                    {
                        #region params
                        DynamicParameters param = new DynamicParameters();
                        param.Add("@ID", item.ID);
                        param.Add("@UserID", userID);
                        param.Add("@PersonID", item.PersonID);
                        param.Add("@SortOrder", item.SortOrder);
                        #endregion

                        string query = $@"
                        DELETE FROM UserFavoritePeopleJunction WHERE UserID = @UserID AND IsActor = 1
                        INSERT INTO UserFavoritePeopleJunction (UserID, PersonID, SortOrder, IsActor)
                        VALUES (@UserID, @PersonID, @SortOrder, 1)
                        SELECT t.ID, t.PersonID, t.SortOrder, m.Name, m.Photo_URL
                        FROM UserFavoritePeopleJunction t
                        LEFT JOIN People m ON m.TMDB_ID = t.PersonID
                        WHERE UserID = @UserID AND IsActor = 1
                        ORDER BY t.SortOrder ASC";

                        var res = await con.QueryFirstOrDefaultAsync<UserFavoritePeople>(query, param);
                        result.Add(res);
                    }
                    con.Dispose();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
            result = result.OrderBy(m => m.ID).ToList();
            return result;
        }
        public async Task<List<UserFavoritePeople>> ManageFavoriteDirectors(List<UserFavoritePeople> entity, int userID)
        {
            var result = new List<UserFavoritePeople>();
            try
            {
                using (var con = GetConnection)
                {
                    foreach (var item in entity)
                    {
                        #region params
                        DynamicParameters param = new DynamicParameters();
                        param.Add("@ID", item.ID);
                        param.Add("@UserID", userID);
                        param.Add("@PersonID", item.PersonID);
                        param.Add("@SortOrder", item.SortOrder);
                        #endregion

                        string query = $@"
                        DELETE FROM UserFavoritePeopleJunction WHERE UserID = @UserID AND IsActor = 0
                        INSERT INTO UserFavoritePeopleJunction (UserID, PersonID, SortOrder, IsActor)
                        VALUES (@UserID, @PersonID, @SortOrder, 0)
                        SELECT t.ID, t.PersonID, t.SortOrder, m.Name, m.Photo_URL
                        FROM UserFavoritePeopleJunction t
                        LEFT JOIN People m ON m.TMDB_ID = t.PersonID
                        WHERE UserID = @UserID AND IsActor = 0
                        ORDER BY t.SortOrder ASC";

                        var res = await con.QueryFirstOrDefaultAsync<UserFavoritePeople>(query, param);
                        result.Add(res);
                    }
                    con.Dispose();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
            result = result.OrderBy(m => m.ID).ToList();
            return result;
        }
    }
}
