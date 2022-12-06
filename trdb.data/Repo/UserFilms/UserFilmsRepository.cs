using Dapper;
using trdb.entity.Returns;
using trdb.entity.UserFilms;
using trdb.data.Interface.UserFilms;
using trdb.entity.Films;

namespace trdb.data.Repo.UserFilms
{
    public class UserFilmsRepository : Connection.dbConnection, IUserFilms
    {
        public async Task<UserLogsReturn> GetUserFilmLogs(string username, string title, string year)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@username", username);
            param.Add("@title", title);
            param.Add("@year", year);

            string filmQuery = @"
            SELECT *
            FROM Films t
            WHERE t.Title LIKE '%' + @title + '%' AND t.Release_Date LIKE '%' + @year + '%'";
            string reviewQuery = @"
            SELECT t.*, u.ID as UserID,
	            (SELECT CASE WHEN EXISTS (
                        SELECT *
                        FROM UserLikedFilmsJunction
                        WHERE FilmID = @FilmID AND UserID in (Select ID from Users Where ID = u.ID)
                 ) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END) as Liked
            FROM UserFilmReviewJunction t
            LEFT JOIN Users as u on Username Like '%' + @username + '%'
            WHERE t.FilmID = @FilmID AND  t.UserID = u.ID
            ORDER BY t.ID DESC";

            using (var con = GetConnection)
            {
                var result = new UserLogsReturn();
                result.Film = await con.QueryFirstOrDefaultAsync<FilmReturn>(filmQuery, param);
                param.Add("@FilmID", result.Film.TMDB_ID);
                result.Reviews = await con.QueryAsync<UserFilmReviews>(reviewQuery, param);
                return result;
            }
        }
        public async Task<UserReviewReturn> GetUserFilmReview(string username, string title, string year, int? count)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@username", username);
            param.Add("@title", title);
            param.Add("@year", year);
            param.Add("@ID", count);

            string whereClause = @"WHERE t.FilmID = @FilmID AND t.UserID = u.ID";
            if (count.HasValue)
            {
                whereClause += " AND t.ID = @ID";
            }

            string filmQuery = @"
            SELECT *
            FROM Films t
            WHERE t.Title LIKE '%' + @title + '%' AND t.Release_Date LIKE '%' + @year + '%'";
            string reviewQuery = $@"
            SELECT TOP (1) t.*, u.ID as UserID,
	            (SELECT CASE WHEN EXISTS (
                        SELECT *
                        FROM UserLikedFilmsJunction
                        WHERE FilmID = @FilmID AND UserID in (Select ID from Users Where ID = u.ID)
                 ) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END) as Liked
            FROM UserFilmReviewJunction t
            LEFT JOIN Users as u on Username Like '%' + @username + '%'
            {whereClause} ";

            using (var con = GetConnection)
            {
                var result = new UserReviewReturn();
                result.Film = await con.QueryFirstOrDefaultAsync<FilmReturn>(filmQuery, param);
                param.Add("@FilmID", result.Film.TMDB_ID);
                result.Review = await con.QueryFirstOrDefaultAsync<UserFilmReviews>(reviewQuery, param);
                return result;
            }
        }

        public async Task<UserFilmReturn> GetUserFilmDetails(int ID, int UserID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ID", ID);
            param.Add("@UserID", UserID);

            string Query = @"
            SELECT 
            (
	             SELECT CASE WHEN EXISTS (
                            SELECT *
                            FROM UserWatchedFilmsJunction
                            WHERE FilmID = @ID AND UserID = @UserID
                        )
                        THEN CAST(1 AS BIT)
                        ELSE CAST(0 AS BIT) END 
            ) as Watched,
            (
	             SELECT CASE WHEN EXISTS (
                            SELECT *
                            FROM UserWatchlistJunction
                            WHERE FilmID = @ID AND UserID = @UserID
                        )
                        THEN CAST(1 AS BIT)
                        ELSE CAST(0 AS BIT) END 
            ) as Watchlist,
            (
	             SELECT CASE WHEN EXISTS (
                            SELECT *
                            FROM UserLikedFilmsJunction
                            WHERE FilmID = @ID AND UserID = @UserID
                        )
                        THEN CAST(1 AS BIT)
                        ELSE CAST(0 AS BIT) END 
            ) as Liked";

            string ratingQuery = @"
            SELECT * 
            FROM UserFilmRatingsJunction t
            WHERE FilmID = @ID AND UserID = @UserID";

            string reviewsQuery = @"
            SELECT * 
            FROM UserFilmReviewJunction t
            WHERE FilmID = @ID AND UserID = @UserID";

            using (var con = GetConnection)
            {
                var res = await con.QueryFirstOrDefaultAsync<UserFilmReturn>(Query, param);
                res.Rating = await con.QueryFirstOrDefaultAsync<UserFilmRatings>(ratingQuery, param);
                res.Reviews = await con.QueryAsync<UserFilmReviews>(reviewsQuery, param);
                return res;
            }
        }

        public async Task<UserFilmReviews> ManageReview(UserFilmReviews entity, int UserID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ID", entity.ID);
            param.Add("@UserID", UserID);
            param.Add("@FilmID", entity.FilmID);
            param.Add("@Rating", entity.Rating);
            param.Add("@Review", entity.Review);
            param.Add("@IsRewatch", entity.IsRewatch);
            param.Add("@Date", entity.Date);

            string Query = @"
            DECLARE @result table(ID Int, UserID Int, FilmID Int, Rating decimal(2,1), Review nvarchar(MAX), Date smalldatetime, IsRewatch bit)
            IF EXISTS(SELECT * from UserFilmReviewJunction where ID = @ID)        
            BEGIN            
                UPDATE UserFilmReviewJunction
                SET Rating = @Rating, Review = @Review, IsRewatch = @IsRewatch
                OUTPUT INSERTED.* INTO @result
                WHERE ID = @ID;
            END                    
            ELSE            
            BEGIN  
	            INSERT INTO UserFilmReviewJunction (FilmID, UserID, Rating, Review, Date, IsRewatch)
                OUTPUT INSERTED.* INTO @result
	            VALUES (@FilmID, @UserID, @Rating, @Review, @Date, @IsRewatch)
            END
            SELECT *
			FROM @result";
            string setWatched = @"
            IF NOT EXISTS(SELECT * from UserWatchedFilmsJunction where FilmID = @FilmID AND UserID = @UserID)        
            BEGIN  
	            INSERT INTO UserWatchedFilmsJunction (FilmID, UserID, Date)
	            VALUES (@FilmID, @UserID, GETDATE())
            END
            SELECT CASE WHEN EXISTS (
                SELECT *
                FROM UserWatchedFilmsJunction
                WHERE FilmID = @FilmID AND UserID = @UserID
            )
            THEN CAST(1 AS BIT)
            ELSE CAST(0 AS BIT) END";
            string setLike = @"
            IF NOT EXISTS(SELECT * from UserLikedFilmsJunction where FilmID = @FilmID AND UserID = @UserID)            
            BEGIN  
	            INSERT INTO UserLikedFilmsJunction (FilmID, UserID, Date)
	            VALUES (@FilmID, @UserID, GETDATE())
            END
            SELECT CASE WHEN EXISTS (
                SELECT *
                FROM UserLikedFilmsJunction
                WHERE FilmID = @FilmID AND UserID = @UserID
            )
            THEN CAST(1 AS BIT)
            ELSE CAST(0 AS BIT) END";
            string removeLike = @"      
	        DELETE FROM UserLikedFilmsJunction WHERE FilmID = @FilmID AND UserID = @UserID
            SELECT CASE WHEN EXISTS (
                SELECT *
                FROM UserLikedFilmsJunction
                WHERE FilmID = @FilmID AND UserID = @UserID
            )
            THEN CAST(1 AS BIT)
            ELSE CAST(0 AS BIT) END";
            using (var con = GetConnection)
            {
                var res = await con.QueryFirstOrDefaultAsync<UserFilmReviews>(Query, param);
                res.Watched = await con.QueryFirstOrDefaultAsync<bool>(setWatched, param);
                if (entity.Liked)
                {
                    res.Liked = await con.QueryFirstOrDefaultAsync<bool>(setLike, param);
                }
                else
                {
                    res.Liked = await con.QueryFirstOrDefaultAsync<bool>(removeLike, param);
                }

                return res;
            }
        }

        public async Task<UserFilmRatings> ManageRatings(UserFilmRatings entity, int UserID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ID", entity.ID);
            param.Add("@FilmID", entity.FilmID);
            param.Add("@Rating", entity.Rating);
            param.Add("@UserID", UserID);

            string Query = @"
            DECLARE @result table(ID Int, UserID Int, FilmID Int, Rating decimal(2,1), Date smalldatetime)
            IF EXISTS(SELECT * from UserFilmRatingsJunction where ID = @ID)        
            BEGIN            
                UPDATE UserFilmRatingsJunction
                SET Rating = @Rating, Date = GETDATE()
                OUTPUT INSERTED.* INTO @result
                WHERE ID = @ID;
            END                    
            ELSE            
            BEGIN  
	            INSERT INTO UserFilmRatingsJunction (FilmID, UserID, Rating, Date)
                OUTPUT INSERTED.* INTO @result
	            VALUES (@FilmID, @UserID, @Rating, GETDATE())
            END
            SELECT *
			FROM @result";

            using (var con = GetConnection)
            {
                var res = await con.QueryAsync<UserFilmRatings>(Query, param);
                return res.FirstOrDefault();
            }
        }

        public async Task<bool> ToggleLike(int FilmID, int UserID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@FilmID", FilmID);
            param.Add("@UserID", UserID);

            string Query = @"
            IF EXISTS(SELECT * from UserLikedFilmsJunction where FilmID = @FilmID AND UserID = @UserID)        
            BEGIN            
	            DELETE FROM UserLikedFilmsJunction WHERE FilmID = @FilmID AND UserID = @UserID
            END                    
            ELSE            
            BEGIN  
	            INSERT INTO UserLikedFilmsJunction (FilmID, UserID, Date)
	            VALUES (@FilmID, @UserID, GETDATE())
            END
            SELECT CASE WHEN EXISTS (
                SELECT *
                FROM UserLikedFilmsJunction
                WHERE FilmID = @FilmID AND UserID = @UserID
            )
            THEN CAST(1 AS BIT)
            ELSE CAST(0 AS BIT) END";

            using (var con = GetConnection)
            {
                var res = await con.QueryAsync<bool>(Query, param);
                return res.FirstOrDefault();
            }
        }

        public async Task<bool> ToggleWatched(int FilmID, int UserID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@FilmID", FilmID);
            param.Add("@UserID", UserID);

            string Query = @"
            IF EXISTS(SELECT * from UserWatchedFilmsJunction where FilmID = @FilmID AND UserID = @UserID)        
            BEGIN            
	            DELETE FROM UserWatchedFilmsJunction WHERE FilmID = @FilmID AND UserID = @UserID
            END                    
            ELSE            
            BEGIN  
	            INSERT INTO UserWatchedFilmsJunction (FilmID, UserID, Date)
	            VALUES (@FilmID, @UserID, GETDATE())
            END
            SELECT CASE WHEN EXISTS (
                SELECT *
                FROM UserWatchedFilmsJunction
                WHERE FilmID = @FilmID AND UserID = @UserID
            )
            THEN CAST(1 AS BIT)
            ELSE CAST(0 AS BIT) END";

            using (var con = GetConnection)
            {
                var res = await con.QueryAsync<bool>(Query, param);
                return res.FirstOrDefault();
            }
        }

        public async Task<bool> ToggleWatchlist(int FilmID, int UserID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@FilmID", FilmID);
            param.Add("@UserID", UserID);

            string Query = @"
            IF EXISTS(SELECT * from UserWatchlistJunction where FilmID = @FilmID AND UserID = @UserID)        
            BEGIN            
	            DELETE FROM UserWatchlistJunction WHERE FilmID = @FilmID AND UserID = @UserID
            END                    
            ELSE            
            BEGIN  
	            INSERT INTO UserWatchlistJunction (FilmID, UserID, Date)
	            VALUES (@FilmID, @UserID, GETDATE())
            END
            SELECT CASE WHEN EXISTS (
                SELECT *
                FROM UserWatchlistJunction
                WHERE FilmID = @FilmID AND UserID = @UserID
            )
            THEN CAST(1 AS BIT)
            ELSE CAST(0 AS BIT) END";

            using (var con = GetConnection)
            {
                var res = await con.QueryAsync<bool>(Query, param);
                return res.FirstOrDefault();
            }
        }
    }
}
