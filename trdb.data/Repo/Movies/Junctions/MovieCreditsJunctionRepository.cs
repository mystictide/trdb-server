using Dapper.Contrib.Extensions;
using Dapper;
using trdb.data.Interface.Movies.Junctions;
using trdb.data.Repo.Helpers;
using trdb.entity.Helpers;
using trdb.entity.Movies.Junctions;
using trdb.entity.Movies;

namespace trdb.data.Repo.Movies.Junctions
{
    public class MovieCreditsJunctionRepository : Connection.dbConnection, IMovieCreditsJunction
    {
        public async Task<MovieCreditsJunction> Add(MovieCreditsJunction entity)
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
                    await con.DeleteAsync(new MovieCreditsJunction() { ID = ID });
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

        public async Task<FilteredList<MovieCreditsJunction>> FilteredList(FilteredList<MovieCreditsJunction> request)
        {
            try
            {
                FilteredList<MovieCreditsJunction> result = new FilteredList<MovieCreditsJunction>();
                DynamicParameters param = new DynamicParameters();
                param.Add("@Keyword", request.filter.Keyword);
                param.Add("@PageSize", request.filter.pageSize);

                string WhereClause = @" WHERE (t.Name like '%' + @Keyword + '%')";

                string query_count = $@"  Select Count(t.ID) from MovieCreditsJunction t {WhereClause}";

                string query = $@"
                SELECT *
                FROM MovieCreditsJunction t
                {WhereClause} 
                ORDER BY t.ID ASC 
                OFFSET @StartIndex ROWS
                FETCH NEXT @PageSize ROWS ONLY";

                using (var con = GetConnection)
                {
                    result.totalItems = await con.QueryFirstOrDefaultAsync<int>(query_count, param);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    param.Add("@StartIndex", request.filter.pager.StartIndex);
                    result.data = await con.QueryAsync<MovieCreditsJunction>(query, param);
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

        public async Task<MovieCreditsJunction> Get(int ID)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ID", ID);

                string query = $@"
                SELECT *
                FROM MovieCreditsJunction 
                WHERE ID = @ID";

                using (var con = GetConnection)
                {
                    var res = await con.QueryAsync<MovieCreditsJunction>(query, param);
                    return res.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<IEnumerable<MovieCreditsJunction>> GetAll()
        {
            try
            {
                using (var con = GetConnection)
                {
                    var res = await con.GetAllAsync<MovieCreditsJunction>();
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<List<People>> Manage(List<People> entity, int MovieID)
        {
            var result = new List<People>();
            foreach (var item in entity)
            {
                try
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@MovieID", MovieID);
                    param.Add("@PersonID", item.TMDB_ID);
                    param.Add("@Character", item.Character);
                    param.Add("@Department", item.Department);
                    param.Add("@Job", item.Job);
                    param.Add("@ListOrder", item.ListOrder);

                    string query = $@"
                    DECLARE  @result table(ID Int, MovieID Int, PersonID Int, Character nvarchar(MAX), Department nvarchar(250), Job nvarchar(250), ListOrder Int)
                    IF EXISTS(SELECT * from MovieCreditsJunction where MovieID = @MovieID AND PersonID = @PersonID)        
                    BEGIN            
                    UPDATE MovieCreditsJunction
                                SET MovieID = @MovieID, PersonID = @PersonID, Character = @Character, Department = @Department, Job = @Job, ListOrder = @ListOrder
							    OUTPUT INSERTED.* INTO @result
                                WHERE MovieID = @MovieID AND PersonID = @PersonID;
                    END                    
                    ELSE            
                    BEGIN  
                    INSERT INTO MovieCreditsJunction (MovieID, PersonID, Character, Department, Job, ListOrder)
                                 OUTPUT INSERTED.* INTO @result
                                 VALUES (@MovieID, @PersonID, @Character, @Department, @Job, @ListOrder)
                    END
                    SELECT *
				    FROM @result";

                    using (var con = GetConnection)
                    {
                        var res = await con.QueryFirstOrDefaultAsync<MovieCreditsJunction>(query, param);
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

        public async Task<ProcessResult> Update(MovieCreditsJunction entity)
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
