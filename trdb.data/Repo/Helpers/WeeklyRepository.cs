using Dapper;
using trdb.data.Interface.Helpers;

namespace trdb.data.Repo.Helpers
{
    public class WeeklyRepository : Connection.dbConnection, IWeekly
    {
        public async Task<entity.Films.Films> Manage()
        {
            try
            {
                string query = $@"
                IF NOT EXISTS(SELECT TOP 1 ID from Weekly WHERE Date BETWEEN DATEADD(day, -7, GETDATE()) AND GETDATE() ORDER BY ID DESC)
                BEGIN            
                INSERT INTO Weekly (FilmID, Date)
	                VALUES ((SELECT TOP 1 TMDB_ID from Films WHERE Backdrop_URL IS NOT NULL  AND IsAdult = 0 
                        ORDER BY NEWID()), GETDATE())
	                SELECT TOP 1 ID, TMDB_ID, Title, Backdrop_URL, Poster_URL 
                    ,(select Date from Weekly WHERE FilmID = t.TMDB_ID) as WeeklyExpiryDate
                    FROM Films as t WHERE t.TMDB_ID in
		                (SELECT TOP 1 FilmID from Weekly WHERE FilmID = t.TMDB_ID ORDER BY ID DESC) ORDER BY ID DESC
                END
                ELSE
                BEGIN        
                SELECT TOP 1 ID, TMDB_ID, Title, Backdrop_URL, Poster_URL, Release_Date 
                ,(select Date from Weekly WHERE FilmID = t.TMDB_ID) as WeeklyExpiryDate
                FROM Films as t WHERE t.TMDB_ID in
	                (SELECT TOP 1 FilmID from Weekly WHERE FilmID = t.TMDB_ID ORDER BY ID DESC) ORDER BY ID DESC
                END";

                using (var con = GetConnection)
                {
                    var res = await con.QueryFirstOrDefaultAsync<entity.Films.Films>(query);
                    return res;
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
