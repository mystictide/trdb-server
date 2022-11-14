using Dapper;
using trdb.data.Interface.UserFilms;
using trdb.entity.Films;
using trdb.entity.UserFilms;

namespace trdb.data.Repo.User
{
    public class UserFilmsRepository : Connection.dbConnection, IUserFilms
    {
        public async Task<UserFilmReviews> ManageReview(UserFilmReviews entity, int UserID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ID", entity.ID);
            param.Add("@UserID", UserID);
            param.Add("@FilmID", entity.FilmID);
            param.Add("@Rating", entity.Rating);
            param.Add("@Review", entity.Review);
            param.Add("@Date", entity.Date);

            string Query = @"
            DECLARE @result table(ID Int, UserID Int, FilmID Int, Rating decimal(2,1), Review nvarchar(MAX), Date smalldatetime)
            IF EXISTS(SELECT * from UserFilmReviewJunction where ID = @ID)        
            BEGIN            
                UPDATE UserFilmReviewJunction
                SET Rating = @Rating, Review = @Review, Date = @Date
                OUTPUT INSERTED.* INTO @result
                WHERE ID = @ID;
            END                    
            ELSE            
            BEGIN  
	            INSERT INTO UserFilmReviewJunction (FilmID, UserID, Rating, Review, Date)
                OUTPUT INSERTED.* INTO @result
	            VALUES (@FilmID, @UserID, @Rating, @Review, @Date)
            END
            SELECT *
			FROM @result";

            using (var con = GetConnection)
            {
                var res = await con.QueryFirstOrDefaultAsync<UserFilmReviews>(Query, param);
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
