using Dapper;
using trdb.data.Interface.UserMovies;
using trdb.entity.Movies;
using trdb.entity.UserMovies;

namespace trdb.data.Repo.User
{
    public class UserMoviesRepository : Connection.dbConnection, IUserMovies
    {
        public async Task<UserMovieReviews> ManageReview(UserMovieReviews entity, int UserID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ID", entity.ID);
            param.Add("@UserID", UserID);
            param.Add("@MovieID", entity.MovieID);
            param.Add("@Rating", entity.Rating);
            param.Add("@Review", entity.Review);
            param.Add("@Date", entity.Date);

            string Query = @"
            DECLARE @result table(ID Int, UserID Int, MovieID Int, Rating decimal(2,1), Review nvarchar(MAX), Date smalldatetime)
            IF EXISTS(SELECT * from UserMovieReviewJunction where ID = @ID)        
            BEGIN            
                UPDATE UserMovieReviewJunction
                SET Rating = @Rating, Review = @Review, Date = @Date
                OUTPUT INSERTED.* INTO @result
                WHERE ID = @ID;
            END                    
            ELSE            
            BEGIN  
	            INSERT INTO UserMovieReviewJunction (MovieID, UserID, Rating, Review, Date)
                OUTPUT INSERTED.* INTO @result
	            VALUES (@MovieID, @UserID, @Rating, @Review, @Date)
            END
            SELECT *
			FROM @result";

            using (var con = GetConnection)
            {
                var res = await con.QueryFirstOrDefaultAsync<UserMovieReviews>(Query, param);
                return res;
            }
        }

        public async Task<UserMovieRatings> ManageRatings(UserMovieRatings entity, int UserID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@ID", entity.ID);
            param.Add("@MovieID", entity.MovieID);
            param.Add("@Rating", entity.Rating);
            param.Add("@UserID", UserID);

            string Query = @"
            DECLARE @result table(ID Int, UserID Int, MovieID Int, Rating decimal(2,1), Date smalldatetime)
            IF EXISTS(SELECT * from UserMovieRatingsJunction where ID = @ID)        
            BEGIN            
                UPDATE UserMovieRatingsJunction
                SET Rating = @Rating, Date = GETDATE()
                OUTPUT INSERTED.* INTO @result
                WHERE ID = @ID;
            END                    
            ELSE            
            BEGIN  
	            INSERT INTO UserMovieRatingsJunction (MovieID, UserID, Rating, Date)
                OUTPUT INSERTED.* INTO @result
	            VALUES (@MovieID, @UserID, @Rating, GETDATE())
            END
            SELECT *
			FROM @result";

            using (var con = GetConnection)
            {
                var res = await con.QueryAsync<UserMovieRatings>(Query, param);
                return res.FirstOrDefault();
            }
        }

        public async Task<bool> ToggleLike(int MovieID, int UserID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@MovieID", MovieID);
            param.Add("@UserID", UserID);

            string Query = @"
            IF EXISTS(SELECT * from UserLikedMoviesJunction where MovieID = @MovieID AND UserID = @UserID)        
            BEGIN            
	            DELETE FROM UserLikedMoviesJunction WHERE MovieID = @MovieID AND UserID = @UserID
            END                    
            ELSE            
            BEGIN  
	            INSERT INTO UserLikedMoviesJunction (MovieID, UserID, Date)
	            VALUES (@MovieID, @UserID, GETDATE())
            END
            SELECT CASE WHEN EXISTS (
                SELECT *
                FROM UserLikedMoviesJunction
                WHERE MovieID = @MovieID AND UserID = @UserID
            )
            THEN CAST(1 AS BIT)
            ELSE CAST(0 AS BIT) END";

            using (var con = GetConnection)
            {
                var res = await con.QueryAsync<bool>(Query, param);
                return res.FirstOrDefault();
            }
        }

        public async Task<bool> ToggleWatched(int MovieID, int UserID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@MovieID", MovieID);
            param.Add("@UserID", UserID);

            string Query = @"
            IF EXISTS(SELECT * from UserWatchedMoviesJunction where MovieID = @MovieID AND UserID = @UserID)        
            BEGIN            
	            DELETE FROM UserWatchedMoviesJunction WHERE MovieID = @MovieID AND UserID = @UserID
            END                    
            ELSE            
            BEGIN  
	            INSERT INTO UserWatchedMoviesJunction (MovieID, UserID, Date)
	            VALUES (@MovieID, @UserID, GETDATE())
            END
            SELECT CASE WHEN EXISTS (
                SELECT *
                FROM UserWatchedMoviesJunction
                WHERE MovieID = @MovieID AND UserID = @UserID
            )
            THEN CAST(1 AS BIT)
            ELSE CAST(0 AS BIT) END";

            using (var con = GetConnection)
            {
                var res = await con.QueryAsync<bool>(Query, param);
                return res.FirstOrDefault();
            }
        }

        public async Task<bool> ToggleWatchlist(int MovieID, int UserID)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@MovieID", MovieID);
            param.Add("@UserID", UserID);

            string Query = @"
            IF EXISTS(SELECT * from UserWatchlistJunction where MovieID = @MovieID AND UserID = @UserID)        
            BEGIN            
	            DELETE FROM UserWatchlistJunction WHERE MovieID = @MovieID AND UserID = @UserID
            END                    
            ELSE            
            BEGIN  
	            INSERT INTO UserWatchlistJunction (MovieID, UserID, Date)
	            VALUES (@MovieID, @UserID, GETDATE())
            END
            SELECT CASE WHEN EXISTS (
                SELECT *
                FROM UserWatchlistJunction
                WHERE MovieID = @MovieID AND UserID = @UserID
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
