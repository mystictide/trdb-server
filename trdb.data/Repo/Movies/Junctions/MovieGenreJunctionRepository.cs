using Dapper.Contrib.Extensions;
using Dapper;
using trdb.data.Interface.Movies.Junctions;
using trdb.data.Repo.Helpers;
using trdb.entity.Helpers;
using trdb.entity.Movies.Junctions;

namespace trdb.data.Repo.Movies.Junctions
{
    public class MovieGenreJunctionRepository : Connection.dbConnection, IMovieGenreJunction
    {
        public async Task<MovieGenreJunction> Add(MovieGenreJunction entity)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (var con = GetConnection)
                {
                    result.ReturnID = await con.InsertAsync(entity);
                    result.Message = "Item saved successfully";
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
                    await con.DeleteAsync(new MovieGenreJunction() { ID = ID });
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

        public async Task<FilteredList<MovieGenreJunction>> FilteredList(FilteredList<MovieGenreJunction> request)
        {
            try
            {
                FilteredList<MovieGenreJunction> result = new FilteredList<MovieGenreJunction>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);

                string WhereClause = @" WHERE (t.Name like '%' + @Keyword + '%')";

                string query_count = $@"  Select Count(t.ID) from MovieGenreJunction t {WhereClause}";

                string query = $@"
                SELECT *
                FROM MovieGenreJunction t
                {WhereClause} 
                ORDER BY t.ID ASC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var con = GetConnection)
                {
                    result.totalItems = await con.QueryFirstOrDefaultAsync<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = await con.QueryAsync<MovieGenreJunction>(query, param);
                    result.filter = request.filter;
                    result.filterModel = request.filterModel;
                    return result;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<MovieGenreJunction> Get(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string query = $@"
                SELECT *
                FROM MovieGenreJunction 
                WHERE ID = @ID";

                using (var con = GetConnection)
                {
                    var res = await con.QueryAsync<MovieGenreJunction>(query, param);
                    return res.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<IEnumerable<MovieGenreJunction>> GetAll()
        {
            try
            {
                using (var con = GetConnection)
                {
                    var res = await con.GetAllAsync<MovieGenreJunction>();
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<List<entity.Movies.MovieGenres>> Manage(List<entity.Movies.MovieGenres> entity, int MovieID)
        {
            var result = new List<entity.Movies.MovieGenres>();
            foreach (var item in entity)
            {
                try
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@MovieID", MovieID);
                    param.Add("@GenreID", item.TMDB_ID);

                    string query = $@"
                    DECLARE  @result table(ID Int, MovieID Int, GenreID Int)
                    IF EXISTS(SELECT * from MovieGenreJunction where MovieID = @MovieID AND GenreID = @GenreID)        
                    BEGIN            
                    UPDATE MovieGenreJunction
                                SET MovieID = @MovieID, GenreID = @GenreID
							    OUTPUT INSERTED.* INTO @result
                                WHERE MovieID = @MovieID AND GenreID = @GenreID;
                    END                    
                    ELSE            
                    BEGIN  
                    INSERT INTO MovieGenreJunction (MovieID, GenreID)
                                 OUTPUT INSERTED.* INTO @result
                                 VALUES (@MovieID, @GenreID)
                    END
                    SELECT *
				    FROM @result";

                    using (var con = GetConnection)
                    {
                        var res = await con.QueryFirstOrDefaultAsync<MovieGenreJunction>(query, param);
                        item.ID = res.ID;
                        result.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    LogsRepository.CreateLog(ex);
                }
            }
            return result;
        }

        public async Task<ProcessResult> Update(MovieGenreJunction entity)
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
                        result.Message = "Item updated successfully.";
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
