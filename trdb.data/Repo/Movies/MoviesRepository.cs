using Dapper.Contrib.Extensions;
using Dapper;
using trdb.data.Interface.Movies;
using trdb.data.Repo.Helpers;
using trdb.entity.Helpers;
using trdb.entity.Movies;

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

                string WhereClause = @" WHERE (t.Name like '%' + @Keyword + '%')";

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
                FROM Movies 
                WHERE ID = @ID";

                using (var con = GetConnection)
                {
                    var res = await con.QueryAsync<entity.Movies.Movies>(query, param);
                    return res.FirstOrDefault();
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

        public async Task<entity.Movies.Movies> Import(entity.Movies.Movies entity)
        {
            try
            {
                #region params
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", entity.ID);
                param.Add("@TMDB_ID", entity.TMDB_ID);

                #endregion

                string query = $@"
                   DECLARE  @result table(ID Int, TMDB_ID Int, Name nvarchar(MAX), Logo_URL nvarchar(MAX), Origin nvarchar(100))
                    IF EXISTS(SELECT * from ProductionCompanies where TMDB_ID = @TMDB_ID)        
                    BEGIN            
                    UPDATE ProductionCompanies
                                SET TMDB_ID = @TMDB_ID, Name = @Name, Logo_URL = @Logo_URL, Origin = @Origin
							    OUTPUT INSERTED.* INTO @result
                                WHERE ID = @ID AND TMDB_ID = @TMDB_ID;
                    END                    
                    ELSE            
                    BEGIN  
                    INSERT INTO ProductionCompanies (TMDB_ID, Name, Logo_URL, Origin)
                                 OUTPUT INSERTED.* INTO @result
                                 VALUES (@TMDB_ID, @Name, @Logo_URL, @Origin)
                    END
                    SELECT *
				    FROM @result";

                using (var con = GetConnection)
                {
                    var res = await con.QueryFirstOrDefaultAsync<entity.Movies.Movies>(query, param);
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
