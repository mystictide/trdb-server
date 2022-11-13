using Dapper.Contrib.Extensions;
using Dapper;
using trdb.data.Interface.Movies;
using trdb.data.Repo.Helpers;
using trdb.entity.Helpers;
using trdb.data.Repo.MovieGenres;

namespace trdb.data.Repo.Movies
{
    public class MoviesRepository : Connection.dbConnection, IMovies
    {
        public async Task<entity.Movies.Movies> Add(entity.Movies.Movies entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = await con.InsertAsync(entity);
                    result.Message = "Movie saved successfully";
                    result.State = ProcessState.Success;
                    entity.ID = result.ReturnID;
                    return entity;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.State = ProcessState.Error;
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
                    await con.DeleteAsync(new entity.Movies.Movies() { ID = ID });
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

        public async Task<FilteredList<entity.Movies.Movies>> FilteredList(FilteredList<entity.Movies.Movies> request)
        {
            try
            {
                FilteredList<entity.Movies.Movies> result = new FilteredList<entity.Movies.Movies>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);

                string WhereClause = @" WHERE (t.Title like '%' + @Keyword + '%')";
                string query_count = $@"  Select Count(t.ID) from Movies t {WhereClause}";

                string query = $@"
                SELECT *
                FROM Movies t
                {WhereClause} 
                ORDER BY t.ID ASC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var con = GetConnection)
                {
                    result.totalItems = await con.QueryFirstOrDefaultAsync<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = await con.QueryAsync<entity.Movies.Movies>(query, param);
                    result.filter = request.filter;
                    result.filterModel = request.filterModel;
                    result.filterModel.Pager = result.filter.pager;
                    return result;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<entity.Movies.Movies> Get(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string query = $@"
                SELECT *
                FROM Movies t
                WHERE t.ID = @ID";

                using (var con = GetConnection)
                {
                    var res = await con.QueryFirstOrDefaultAsync<entity.Movies.Movies>(query, param);
                    var mgenres = await new MovieGenresRepository().GetMovieGenres(res.ID);
                    var mlanguages = await new LanguagesRepository().GetMovieLanguages(res.ID);
                    var mcompanies = await new ProductionCompaniesRepository().GetMovieCompanies(res.ID);
                    var mcountries = await new ProductionCountriesRepository().GetMovieCountries(res.ID);
                    res.Genres = mgenres;
                    res.Languages = mlanguages;
                    res.Companies = mcompanies;
                    res.Countries = mcountries;
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<IEnumerable<entity.Movies.Movies>> GetAll()
        {
            try
            {
                using (var con = GetConnection)
                {
                    var res = await con.GetAllAsync<entity.Movies.Movies>();
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<int> GetLatestMovie()
        {
            try
            {
                string query = $@"
                SELECT TOP 1 TMDB_ID FROM Movies ORDER BY ID DESC ";

                using (var con = GetConnection)
                {
                    var res = await con.QueryFirstOrDefaultAsync<int>(query);
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return 0;
            }
        }

        public async Task<entity.Movies.Movies> Import(entity.Movies.Movies entity)
        {
            try
            {
                #region params
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", entity.ID);
                param.Add("@TMDB_ID", entity.TMDB_ID);
                param.Add("@IMDB_ID", entity.IMDB_ID);
                param.Add("@Title", entity.Title);
                param.Add("@Budget", entity.Budget);
                param.Add("@Backdrop_URL", entity.Backdrop_URL);
                param.Add("@Poster_URL", entity.Poster_URL);
                param.Add("@Homepage", entity.Homepage);
                param.Add("@Synopsis", entity.Synopsis);
                param.Add("@Runtime", entity.Runtime);
                param.Add("@Release_Date", entity.Release_Date);
                param.Add("@Tagline", entity.Tagline);
                param.Add("@IsAdult", entity.IsAdult);
                #endregion

                string query = $@"
                   DECLARE  @result table(ID Int, TMDB_ID Int, IMDB_ID nvarchar(MAX), Title nvarchar(MAX), Budget decimal(18, 2), Backdrop_URL nvarchar(MAX), Poster_URL nvarchar(MAX), Homepage nvarchar(MAX), Synopsis nvarchar(MAX), Runtime Int, Release_Date nvarchar(50), Tagline nvarchar(MAX), IsAdult bit)
                    IF EXISTS(SELECT * from Movies where TMDB_ID = @TMDB_ID)        
                    BEGIN            
                    UPDATE Movies
                                SET TMDB_ID = @TMDB_ID, IMDB_ID = @IMDB_ID, Title = @Title, Budget = @Budget, Backdrop_URL = @Backdrop_URL, Poster_URL = @Poster_URL, Homepage = @Homepage, Synopsis = @Synopsis, Runtime = @Runtime, Release_Date = @Release_Date, Tagline = @Tagline, IsAdult = @IsAdult
							    OUTPUT INSERTED.* INTO @result
                                WHERE TMDB_ID = @TMDB_ID;
                    END                    
                    ELSE            
                    BEGIN  
                    INSERT INTO Movies (TMDB_ID, IMDB_ID, Title, Budget, Backdrop_URL, Poster_URL, Homepage, Synopsis, Runtime, Release_Date, Tagline, IsAdult)
                                 OUTPUT INSERTED.* INTO @result
                                 VALUES (@TMDB_ID, @IMDB_ID, @Title, @Budget, @Backdrop_URL, @Poster_URL, @Homepage, @Synopsis, @Runtime, @Release_Date, @Tagline, @IsAdult)
                    END
                    SELECT *
				    FROM @result";

                using (var con = GetConnection)
                {
                    var res = await con.QueryFirstOrDefaultAsync<entity.Movies.Movies>(query, param);
                    con.Dispose();
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<ProcessResult> Update(entity.Movies.Movies entity)
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
                        result.Message = "Movie updated successfully.";
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
    }
}
